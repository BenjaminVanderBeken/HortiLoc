using Dapper;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using HortiLoc.Infrastructure.Data;

namespace HortiLoc.Infrastructure.Repositories;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public MaintenanceRepository(
        DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Maintenance>> GetAllAsync()
    {
        const string sql = """
            SELECT
                ma.id AS Id,
                ma.materiel_id AS MaterielId,
                m.nom AS MaterielNom,
                ma.date_debut AS DateDebut,
                ma.date_fin AS DateFin,
                ma.motif AS Motif,
                ma.statut AS Statut,
                ma.date_creation AS DateCreation
            FROM maintenances ma
            INNER JOIN materiels m
                ON m.id = ma.materiel_id
            ORDER BY ma.date_debut DESC, ma.id DESC;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Maintenance>(sql);
    }

    public async Task<Maintenance?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                ma.id AS Id,
                ma.materiel_id AS MaterielId,
                m.nom AS MaterielNom,
                ma.date_debut AS DateDebut,
                ma.date_fin AS DateFin,
                ma.motif AS Motif,
                ma.statut AS Statut,
                ma.date_creation AS DateCreation
            FROM maintenances ma
            INNER JOIN materiels m
                ON m.id = ma.materiel_id
            WHERE ma.id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Maintenance>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(Maintenance maintenance)
    {
        const string sql = """
            INSERT INTO maintenances
            (
                materiel_id,
                date_debut,
                date_fin,
                motif,
                statut
            )
            VALUES
            (
                @MaterielId,
                @DateDebut,
                NULL,
                @Motif,
                @Statut
            );

            SELECT LAST_INSERT_ID();
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            sql,
            maintenance
        );
    }

    public async Task<bool> UpdateStatutAsync(
        int id,
        string statut,
        DateTime? dateFin)
    {
        const string sql = """
            UPDATE maintenances
            SET
                statut = @Statut,
                date_fin = @DateFin
            WHERE id = @Id;
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        var lignes = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                Statut = statut,
                DateFin = dateFin
            }
        );

        return lignes > 0;
    }
}