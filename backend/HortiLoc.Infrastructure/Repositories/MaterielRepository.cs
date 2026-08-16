using Dapper;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using HortiLoc.Infrastructure.Data;

namespace HortiLoc.Infrastructure.Repositories;

public class MaterielRepository : IMaterielRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public MaterielRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Materiel>> GetAllAsync()
    {
        const string sql = """
            SELECT
                m.id AS Id,
                m.categorie_id AS CategorieId,
                c.nom AS CategorieNom,
                m.nom AS Nom,
                m.description AS Description,
                m.image_url AS ImageUrl,
                m.prix_journalier AS PrixJournalier,
                m.quantite_totale AS QuantiteTotale,
                m.quantite_disponible AS QuantiteDisponible,
                m.actif AS Actif,
                m.date_creation AS DateCreation,
                m.date_modification AS DateModification
            FROM materiels m
            INNER JOIN categories c
                ON c.id = m.categorie_id
            ORDER BY m.nom;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Materiel>(sql);
    }

    public async Task<Materiel?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                m.id AS Id,
                m.categorie_id AS CategorieId,
                c.nom AS CategorieNom,
                m.nom AS Nom,
                m.description AS Description,
                m.image_url AS ImageUrl,
                m.prix_journalier AS PrixJournalier,
                m.quantite_totale AS QuantiteTotale,
                m.quantite_disponible AS QuantiteDisponible,
                m.actif AS Actif,
                m.date_creation AS DateCreation,
                m.date_modification AS DateModification
            FROM materiels m
            INNER JOIN categories c
                ON c.id = m.categorie_id
            WHERE m.id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Materiel>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(Materiel materiel)
    {
        const string sql = """
            INSERT INTO materiels (
                categorie_id,
                nom,
                description,
                image_url,
                prix_journalier,
                quantite_totale,
                quantite_disponible,
                actif
            )
            VALUES (
                @CategorieId,
                @Nom,
                @Description,
                @ImageUrl,
                @PrixJournalier,
                @QuantiteTotale,
                @QuantiteDisponible,
                @Actif
            );

            SELECT LAST_INSERT_ID();
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, materiel);
    }

    public async Task<bool> UpdateAsync(Materiel materiel)
    {
        const string sql = """
            UPDATE materiels
            SET
                categorie_id = @CategorieId,
                nom = @Nom,
                description = @Description,
                image_url = @ImageUrl,
                prix_journalier = @PrixJournalier,
                quantite_totale = @QuantiteTotale,
                quantite_disponible = @QuantiteDisponible
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int lignesModifiees =
            await connection.ExecuteAsync(sql, materiel);

        return lignesModifiees > 0;
    }

    public async Task<bool> DisableAsync(int id)
    {
        const string sql = """
            UPDATE materiels
            SET actif = FALSE
            WHERE id = @Id
              AND actif = TRUE;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int lignesModifiees = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return lignesModifiees > 0;
    }

    public async Task<bool> ReactivateAsync(int id)
    {
        const string sql = """
            UPDATE materiels
            SET actif = TRUE
            WHERE id = @Id
              AND actif = FALSE;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int lignesModifiees = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return lignesModifiees > 0;
    }
}