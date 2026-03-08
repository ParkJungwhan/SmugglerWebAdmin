using System.Data;
using Npgsql;

namespace SmugglerWebCommon.Data;

public sealed class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
}
