using Npgsql;

namespace SmugglerWebAdmin.Data
{
    /// <summary>
    /// Dapper 쿼리용 PostgreSQL 커넥션 팩토리 인터페이스.
    /// </summary>
    public interface IDbConnectionFactory
    {
        NpgsqlConnection CreateConnection();
    }
}
