using MySql.Data.MySqlClient;

namespace HortiLoc.Infrastructure.Data;

public class DatabaseConnectionFactory
{
    private readonly string _connectionString;

    public DatabaseConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
//Créer une connexion vers MySQL