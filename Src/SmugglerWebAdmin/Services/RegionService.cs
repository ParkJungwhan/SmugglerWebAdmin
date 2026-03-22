using Dapper;
using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// Dapper를 사용해 PostgreSQL에서 리전/환경/권한 데이터를 조회합니다.
    /// </summary>
    public class RegionService : IRegionService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RegionService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Region>> GetAllActiveRegionsAsync()
        {
            const string sql = """
                SELECT "Id", "Name", "DisplayName", "IsActive"
                FROM "Regions"
                WHERE "IsActive" = true
                ORDER BY "Id"
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<Region>(sql);
        }

        public async Task<IEnumerable<RegionEnvironment>> GetEnvironmentsByRegionAsync(int regionId)
        {
            const string sql = """
                SELECT "Id", "RegionId", "Name", "DisplayName", "ToolUrl", "IsActive"
                FROM "RegionEnvironments"
                WHERE "RegionId" = @RegionId AND "IsActive" = true
                ORDER BY "Id"
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<RegionEnvironment>(sql, new { RegionId = regionId });
        }

        public async Task<IEnumerable<int>> GetPermittedEnvironmentIdsAsync(string userId)
        {
            const string sql = """
                SELECT "RegionEnvironmentId"
                FROM "UserRegionPermissions"
                WHERE "UserId" = @UserId
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<int>(sql, new { UserId = userId });
        }
    }
}
