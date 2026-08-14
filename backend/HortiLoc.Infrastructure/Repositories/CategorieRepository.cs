using Dapper;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using HortiLoc.Infrastructure.Data;

namespace HortiLoc.Infrastructure.Repositories;

public class CategorieRepository : ICategorieRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public CategorieRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Categorie>> GetAllAsync()
    {
        const string sql = """
            SELECT
                id AS Id,
                nom AS Nom,
                description AS Description,
                actif AS Actif
            FROM categories
            ORDER BY nom;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Categorie>(sql);
    }

    public async Task<Categorie?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                id AS Id,
                nom AS Nom,
                description AS Description,
                actif AS Actif
            FROM categories
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Categorie>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(Categorie categorie)
    {
        const string sql = """
            INSERT INTO categories
            (
                nom,
                description,
                actif
            )
            VALUES
            (
                @Nom,
                @Description,
                @Actif
            );

            SELECT LAST_INSERT_ID();
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            sql,
            categorie
        );
    }

    public async Task<bool> UpdateAsync(Categorie categorie)
    {
        const string sql = """
            UPDATE categories
            SET
                nom = @Nom,
                description = @Description
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        var lignes = await connection.ExecuteAsync(sql, categorie);

        return lignes > 0;
    }

    public async Task<bool> DisableAsync(int id)
    {
        const string sql = """
            UPDATE categories
            SET actif = FALSE
            WHERE id = @Id
              AND actif = TRUE;
            """;

        using var connection = _connectionFactory.CreateConnection();

        var lignes = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return lignes > 0;
    }

    public async Task<bool> ReactivateAsync(int id)
    {
        const string sql = """
            UPDATE categories
            SET actif = TRUE
            WHERE id = @Id
              AND actif = FALSE;
            """;

        using var connection = _connectionFactory.CreateConnection();

        var lignes = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return lignes > 0;
    }

    public async Task<bool> ExistsByNomAsync(
        string nom,
        int? excludeId = null)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM categories
            WHERE LOWER(nom) = LOWER(@Nom)
              AND (@ExcludeId IS NULL OR id <> @ExcludeId);
            """;

        using var connection = _connectionFactory.CreateConnection();

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Nom = nom,
                ExcludeId = excludeId
            }
        );

        return count > 0;
    }
}