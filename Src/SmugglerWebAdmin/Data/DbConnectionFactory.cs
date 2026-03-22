using Npgsql;

namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// appsettings DefaultConnection 문자열을 사용해 NpgsqlConnection을 생성하는 팩토리.
    /// </summary>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
