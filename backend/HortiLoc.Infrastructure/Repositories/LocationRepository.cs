using Dapper;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using HortiLoc.Infrastructure.Data;

namespace HortiLoc.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public LocationRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        const string sql = """
            SELECT
                l.id AS Id,
                l.client_id AS ClientId,
                c.nom AS ClientNom,
                c.prenom AS ClientPrenom,
                l.date_debut AS DateDebut,
                l.date_fin_prevue AS DateFinPrevue,
                l.date_retour AS DateRetour,
                l.statut AS Statut,
                l.montant_total AS MontantTotal,
                l.notes AS Notes,
                l.date_creation AS DateCreation
            FROM locations l
            INNER JOIN clients c
                ON c.id = l.client_id
            ORDER BY l.date_creation DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Location>(sql);
    }

    public async Task<Location?> GetByIdAsync(int id)
    {
        const string locationSql = """
            SELECT
                l.id AS Id,
                l.client_id AS ClientId,
                c.nom AS ClientNom,
                c.prenom AS ClientPrenom,
                l.date_debut AS DateDebut,
                l.date_fin_prevue AS DateFinPrevue,
                l.date_retour AS DateRetour,
                l.statut AS Statut,
                l.montant_total AS MontantTotal,
                l.notes AS Notes,
                l.date_creation AS DateCreation
            FROM locations l
            INNER JOIN clients c
                ON c.id = l.client_id
            WHERE l.id = @Id;
            """;

        const string detailsSql = """
            SELECT
                dl.id AS Id,
                dl.location_id AS LocationId,
                dl.materiel_id AS MaterielId,
                m.nom AS MaterielNom,
                dl.quantite AS Quantite,
                dl.prix_journalier AS PrixJournalier,
                dl.sous_total AS SousTotal
            FROM details_locations dl
            INNER JOIN materiels m
                ON m.id = dl.materiel_id
            WHERE dl.location_id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        var location = await connection.QuerySingleOrDefaultAsync<Location>(
            locationSql,
            new { Id = id }
        );

        if (location is null)
            return null;

        var details = await connection.QueryAsync<DetailLocation>(
            detailsSql,
            new { Id = id }
        );

        location.Details = details.ToList();

        return location;
    }

    public async Task<int> CreateAsync(Location location)
    {
        const string insertLocationSql = """
            INSERT INTO locations (
                client_id,
                date_debut,
                date_fin_prevue,
                statut,
                montant_total,
                notes
            )
            VALUES (
                @ClientId,
                @DateDebut,
                @DateFinPrevue,
                @Statut,
                @MontantTotal,
                @Notes
            );

            SELECT LAST_INSERT_ID();
            """;

        const string verifierStockSql = """
            SELECT quantite_disponible
            FROM materiels
            WHERE id = @MaterielId
            FOR UPDATE;
            """;

        const string insertDetailSql = """
            INSERT INTO details_locations (
                location_id,
                materiel_id,
                quantite,
                prix_journalier,
                sous_total
            )
            VALUES (
                @LocationId,
                @MaterielId,
                @Quantite,
                @PrixJournalier,
                @SousTotal
            );
            """;

        const string diminuerStockSql = """
            UPDATE materiels
            SET quantite_disponible = quantite_disponible - @Quantite
            WHERE id = @MaterielId
              AND quantite_disponible >= @Quantite;
            """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            int locationId = await connection.ExecuteScalarAsync<int>(
                insertLocationSql,
                location,
                transaction
            );

            foreach (var detail in location.Details)
            {
                int stockDisponible =
                    await connection.ExecuteScalarAsync<int>(
                        verifierStockSql,
                        new { detail.MaterielId },
                        transaction
                    );

                if (stockDisponible < detail.Quantite)
                {
                    throw new InvalidOperationException(
                        $"Stock insuffisant pour {detail.MaterielNom}."
                    );
                }

                detail.LocationId = locationId;

                await connection.ExecuteAsync(
                    insertDetailSql,
                    detail,
                    transaction
                );

                int stockModifie = await connection.ExecuteAsync(
                    diminuerStockSql,
                    new
                    {
                        detail.MaterielId,
                        detail.Quantite
                    },
                    transaction
                );

                if (stockModifie == 0)
                {
                    throw new InvalidOperationException(
                        $"Impossible de réserver le stock pour {detail.MaterielNom}."
                    );
                }
            }

            await transaction.CommitAsync();

            return locationId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ReturnAsync(int id, DateTime dateRetour)
    {
        const string locationSql = """
            SELECT statut
            FROM locations
            WHERE id = @Id
            FOR UPDATE;
            """;

        const string detailsSql = """
            SELECT
                materiel_id AS MaterielId,
                quantite AS Quantite
            FROM details_locations
            WHERE location_id = @Id;
            """;

        const string remettreStockSql = """
            UPDATE materiels
            SET quantite_disponible = quantite_disponible + @Quantite
            WHERE id = @MaterielId;
            """;

        const string retourSql = """
            UPDATE locations
            SET
                statut = 'RETOURNEE',
                date_retour = @DateRetour
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            string? statut =
                await connection.QuerySingleOrDefaultAsync<string>(
                    locationSql,
                    new { Id = id },
                    transaction
                );

            if (statut is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            if (statut == "RETOURNEE")
            {
                throw new InvalidOperationException(
                    "Cette location a déjà été retournée."
                );
            }

            if (statut == "ANNULEE")
            {
                throw new InvalidOperationException(
                    "Une location annulée ne peut pas être retournée."
                );
            }

            var details = await connection.QueryAsync<DetailLocation>(
                detailsSql,
                new { Id = id },
                transaction
            );

            foreach (var detail in details)
            {
                await connection.ExecuteAsync(
                    remettreStockSql,
                    new
                    {
                        detail.MaterielId,
                        detail.Quantite
                    },
                    transaction
                );
            }

            await connection.ExecuteAsync(
                retourSql,
                new
                {
                    Id = id,
                    DateRetour = dateRetour
                },
                transaction
            );

            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

public async Task<IEnumerable<Location>> GetByClientIdAsync(int clientId)
{
    const string sql = """
        SELECT
            l.id AS Id,
            l.client_id AS ClientId,
            c.nom AS ClientNom,
            c.prenom AS ClientPrenom,
            l.date_debut AS DateDebut,
            l.date_fin_prevue AS DateFinPrevue,
            l.date_retour AS DateRetour,
            l.statut AS Statut,
            l.montant_total AS MontantTotal,
            l.notes AS Notes,
            l.date_creation AS DateCreation
        FROM locations l
        INNER JOIN clients c
            ON c.id = l.client_id
        WHERE l.client_id = @ClientId
        ORDER BY l.date_debut DESC, l.id DESC;
        """;

    using var connection =
        _connectionFactory.CreateConnection();

    return await connection.QueryAsync<Location>(
        sql,
        new { ClientId = clientId }
    );
}}