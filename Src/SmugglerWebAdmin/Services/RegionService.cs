using Dapper;
using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>
    /// Dapper를 사용해 PostgreSQL에서 서비스/리전/환경/권한 데이터를 조회합니다.
    /// </summary>
    public class RegionService : IRegionService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RegionService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Service>> GetAllActiveServicesAsync()
        {
            const string sql = """
                SELECT "Id", "Name", "DisplayName", "IsActive"
                FROM "Services"
                WHERE "IsActive" = true
                ORDER BY "Id"
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<Service>(sql);
        }

        public async Task<IEnumerable<ServiceRegion>> GetRegionsByServiceAsync(int serviceId)
        {
            const string sql = """
                SELECT "Id", "ServiceId", "Name", "DisplayName", "IsActive"
                FROM "ServiceRegions"
                WHERE "ServiceId" = @ServiceId AND "IsActive" = true
                ORDER BY "Id"
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<ServiceRegion>(sql, new { ServiceId = serviceId });
        }

        public async Task<IEnumerable<RegionEnvironment>> GetEnvironmentsByRegionAsync(int serviceRegionId)
        {
            const string sql = """
                SELECT "Id", "ServiceRegionId", "Name", "DisplayName", "ToolUrl", "IsActive"
                FROM "RegionEnvironments"
                WHERE "ServiceRegionId" = @ServiceRegionId AND "IsActive" = true
                ORDER BY "Id"
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<RegionEnvironment>(sql, new { ServiceRegionId = serviceRegionId });
        }

        public async Task<IEnumerable<int>> GetPermittedServiceIdsAsync(string userId)
        {
            const string sql = """
                SELECT "ServiceId"
                FROM "UserServicePermissions"
                WHERE "UserId" = @UserId
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<int>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<int>> GetPermittedServiceRegionIdsAsync(string userId)
        {
            const string sql = """
                SELECT "ServiceRegionId"
                FROM "UserServiceRegionPermissions"
                WHERE "UserId" = @UserId
                """;

            await using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<int>(sql, new { UserId = userId });
        }
    }
}
