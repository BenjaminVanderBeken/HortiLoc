using Dapper;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using HortiLoc.Infrastructure.Data;

namespace HortiLoc.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public ClientRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        const string sql = """
            SELECT
                id AS Id,
                nom AS Nom,
                prenom AS Prenom,
                email AS Email,
                telephone AS Telephone,
                adresse AS Adresse,
                actif AS Actif,
                date_creation AS DateCreation,
                date_modification AS DateModification
            FROM clients
            ORDER BY nom, prenom;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Client>(sql);
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                id AS Id,
                nom AS Nom,
                prenom AS Prenom,
                email AS Email,
                telephone AS Telephone,
                adresse AS Adresse,
                actif AS Actif,
                date_creation AS DateCreation,
                date_modification AS DateModification
            FROM clients
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Client client)
    {
        const string sql = """
            INSERT INTO clients (
                nom,
                prenom,
                email,
                telephone,
                adresse,
                actif
            )
            VALUES (
                @Nom,
                @Prenom,
                @Email,
                @Telephone,
                @Adresse,
                @Actif
            );

            SELECT LAST_INSERT_ID();
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, client);
    }

    public async Task<bool> UpdateAsync(Client client)
    {
        const string sql = """
            UPDATE clients
            SET
                nom = @Nom,
                prenom = @Prenom,
                email = @Email,
                telephone = @Telephone,
                adresse = @Adresse
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int lignesModifiees = await connection.ExecuteAsync(sql, client);

        return lignesModifiees > 0;
    }

    public async Task<bool> DisableAsync(int id)
    {
        const string sql = """
            UPDATE clients
            SET actif = FALSE
            WHERE id = @Id
              AND actif = TRUE;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int lignesModifiees = await connection.ExecuteAsync(sql, new { Id = id });

        return lignesModifiees > 0;
    }

    public async Task<bool> ReactivateAsync(int id)
    {
        const string sql = """
            UPDATE clients
            SET actif = TRUE
            WHERE id = @Id
              AND actif = FALSE;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int lignesModifiees = await connection.ExecuteAsync(sql, new { Id = id });

        return lignesModifiees > 0;
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM clients
            WHERE email = @Email
              AND (@ExcludeId IS NULL OR id <> @ExcludeId);
            """;

        using var connection = _connectionFactory.CreateConnection();

        long nombre = await connection.ExecuteScalarAsync<long>(
            sql,
            new
            {
                Email = email,
                ExcludeId = excludeId
            }
        );

        return nombre > 0;
    }
}