using Dapper;
using SmugglerWebAdmin.Data;

namespace SmugglerWebAdmin.Services;

/// <summary>IAdminService의 Dapper + PostgreSQL 구현체.</summary>
public class AdminService : IAdminService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AdminService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        await using var conn = _connectionFactory.CreateConnection();
        var users = await conn.QueryAsync<ApplicationUser>(
            """SELECT * FROM "AspNetUsers" ORDER BY "CreatedAt" DESC""");
        return users.ToList();
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        await using var conn = _connectionFactory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<ApplicationUser>(
            """SELECT * FROM "AspNetUsers" WHERE "Id" = @Id""",
            new { Id = userId });
    }

    public async Task SetAdminAsync(string userId, bool isAdmin)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            """UPDATE "AspNetUsers" SET "IsAdmin" = @IsAdmin WHERE "Id" = @Id""",
            new { IsAdmin = isAdmin, Id = userId });
    }

    public async Task SetSuperAdminAsync(string userId, bool isSuperAdmin)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            """UPDATE "AspNetUsers" SET "IsSuperAdmin" = @IsSuperAdmin WHERE "Id" = @Id""",
            new { IsSuperAdmin = isSuperAdmin, Id = userId });
    }

    public async Task SetMustChangePasswordAsync(string userId, bool value)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            """UPDATE "AspNetUsers" SET "MustChangePassword" = @Value WHERE "Id" = @Id""",
            new { Value = value, Id = userId });
    }

    public async Task SetActiveAsync(string userId, bool isActive)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            """UPDATE "AspNetUsers" SET "IsActive" = @IsActive WHERE "Id" = @Id""",
            new { IsActive = isActive, Id = userId });
    }

    public async Task SetUserPermissionsAsync(
        string userId,
        IEnumerable<int> l1ServiceIds,
        IEnumerable<int> l2RegionIds,
        IEnumerable<int> l3EnvIds)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var tx = await conn.BeginTransactionAsync();

        // 기존 권한 전체 삭제
        await conn.ExecuteAsync(
            """DELETE FROM "UserServicePermissions"           WHERE "UserId" = @UserId""",
            new { UserId = userId }, tx);
        await conn.ExecuteAsync(
            """DELETE FROM "UserServiceRegionPermissions"     WHERE "UserId" = @UserId""",
            new { UserId = userId }, tx);
        await conn.ExecuteAsync(
            """DELETE FROM "UserRegionEnvironmentPermissions" WHERE "UserId" = @UserId""",
            new { UserId = userId }, tx);

        // L1 삽입
        foreach (var serviceId in l1ServiceIds)
        {
            await conn.ExecuteAsync(
                """INSERT INTO "UserServicePermissions" ("UserId", "ServiceId") VALUES (@UserId, @ServiceId)""",
                new { UserId = userId, ServiceId = serviceId }, tx);
        }

        // L2 삽입
        foreach (var regionId in l2RegionIds)
        {
            await conn.ExecuteAsync(
                """INSERT INTO "UserServiceRegionPermissions" ("UserId", "ServiceRegionId") VALUES (@UserId, @ServiceRegionId)""",
                new { UserId = userId, ServiceRegionId = regionId }, tx);
        }

        // L3 삽입
        foreach (var envId in l3EnvIds)
        {
            await conn.ExecuteAsync(
                """INSERT INTO "UserRegionEnvironmentPermissions" ("UserId", "RegionEnvironmentId") VALUES (@UserId, @RegionEnvironmentId)""",
                new { UserId = userId, RegionEnvironmentId = envId }, tx);
        }

        await tx.CommitAsync();
    }
}
