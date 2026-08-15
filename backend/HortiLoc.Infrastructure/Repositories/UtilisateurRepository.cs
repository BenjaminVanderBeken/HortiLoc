using Dapper;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using HortiLoc.Infrastructure.Data;

namespace HortiLoc.Infrastructure.Repositories;

public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public UtilisateurRepository(
        DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Utilisateur?> GetByEmailAsync(string email)
    {
        const string sql = """
            SELECT
                id AS Id,
                client_id AS ClientId,
                email AS Email,
                mot_de_passe_hash AS MotDePasseHash,
                role AS Role,
                actif AS Actif,
                date_creation AS DateCreation
            FROM utilisateurs
            WHERE LOWER(email) = LOWER(@Email);
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Utilisateur>(
            sql,
            new { Email = email }
        );
    }

    public async Task<int> CreateAsync(Utilisateur utilisateur)
    {
        const string sql = """
            INSERT INTO utilisateurs
            (
                client_id,
                email,
                mot_de_passe_hash,
                role,
                actif
            )
            VALUES
            (
                @ClientId,
                @Email,
                @MotDePasseHash,
                @Role,
                @Actif
            );

            SELECT LAST_INSERT_ID();
            """;

        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            sql,
            utilisateur
        );
    }
}