using Dapper;
using SmugglerWebAdmin.Data;
using SmugglerWebCommon.Data;

namespace SmugglerWebAdmin.Services
{
    /// <summary>IRegionAdminService의 Dapper + PostgreSQL 구현체.</summary>
    public class RegionAdminService : IRegionAdminService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RegionAdminService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ── L1: Service ───────────────────────────────────────────────────────

        public async Task<List<Service>> GetAllServicesAsync()
        {
            await using var conn = _connectionFactory.CreateConnection();
            var result = await conn.QueryAsync<Service>(
                """SELECT "Id","Name","DisplayName","IsActive" FROM "Services" ORDER BY "Id" """);
            return result.ToList();
        }

        public async Task AddServiceAsync(string name, string displayName)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                """INSERT INTO "Services" ("Name","DisplayName","IsActive") VALUES (@Name,@DisplayName,true)""",
                new { Name = name, DisplayName = displayName });
        }

        public async Task UpdateServiceAsync(int id, string name, string displayName, bool isActive)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                """UPDATE "Services" SET "Name"=@Name,"DisplayName"=@DisplayName,"IsActive"=@IsActive WHERE "Id"=@Id""",
                new { Id = id, Name = name, DisplayName = displayName, IsActive = isActive });
        }

        // ── L2: ServiceRegion ──────────────────────────────────────────────────

        public async Task<List<ServiceRegion>> GetAllRegionsByServiceAsync(int serviceId)
        {
            await using var conn = _connectionFactory.CreateConnection();
            var result = await conn.QueryAsync<ServiceRegion>(
                """SELECT "Id","ServiceId","Name","DisplayName","IsActive" FROM "ServiceRegions" WHERE "ServiceId"=@ServiceId ORDER BY "Id" """,
                new { ServiceId = serviceId });
            return result.ToList();
        }

        public async Task AddRegionAsync(int serviceId, string name, string displayName)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                """INSERT INTO "ServiceRegions" ("ServiceId","Name","DisplayName","IsActive") VALUES (@ServiceId,@Name,@DisplayName,true)""",
                new { ServiceId = serviceId, Name = name, DisplayName = displayName });
        }

        public async Task UpdateRegionAsync(int id, string name, string displayName, bool isActive)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                """UPDATE "ServiceRegions" SET "Name"=@Name,"DisplayName"=@DisplayName,"IsActive"=@IsActive WHERE "Id"=@Id""",
                new { Id = id, Name = name, DisplayName = displayName, IsActive = isActive });
        }

        // ── L3: RegionEnvironment ──────────────────────────────────────────────

        public async Task<List<RegionEnvironment>> GetAllEnvironmentsByRegionAsync(int serviceRegionId)
        {
            await using var conn = _connectionFactory.CreateConnection();
            var result = await conn.QueryAsync<RegionEnvironment>(
                """SELECT "Id","ServiceRegionId","Name","DisplayName","ToolUrl","IsActive" FROM "RegionEnvironments" WHERE "ServiceRegionId"=@ServiceRegionId ORDER BY "Id" """,
                new { ServiceRegionId = serviceRegionId });
            return result.ToList();
        }

        public async Task AddEnvironmentAsync(int serviceRegionId, string name, string displayName, string toolUrl)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                """INSERT INTO "RegionEnvironments" ("ServiceRegionId","Name","DisplayName","ToolUrl","IsActive") VALUES (@ServiceRegionId,@Name,@DisplayName,@ToolUrl,true)""",
                new { ServiceRegionId = serviceRegionId, Name = name, DisplayName = displayName, ToolUrl = toolUrl });
        }

        public async Task UpdateEnvironmentAsync(int id, string name, string displayName, string toolUrl, bool isActive)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                """UPDATE "RegionEnvironments" SET "Name"=@Name,"DisplayName"=@DisplayName,"ToolUrl"=@ToolUrl,"IsActive"=@IsActive WHERE "Id"=@Id""",
                new { Id = id, Name = name, DisplayName = displayName, ToolUrl = toolUrl, IsActive = isActive });
        }
    }
}
