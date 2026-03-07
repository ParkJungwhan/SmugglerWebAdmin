using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace SmugglerWebCommon.DB;

public class PostgresqlDBConnectionFactory
{
    private readonly IConfiguration _config;

    public PostgresqlDBConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection CreateConnection()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return new NpgsqlConnection(_config.GetConnectionString("Postgres"));
    }
}