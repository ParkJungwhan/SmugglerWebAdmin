using System.Security.Claims;
using Dapper;
using SmugglerWebCommon.Auth.Models;
using SmugglerWebCommon.Data;

namespace SmugglerSelectWeb.Services.Auth;

public interface IRegionAccessService
{
    Task<IReadOnlyList<RegionOption>> GetAvailableRegionsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
}

public sealed class DapperRegionAccessService(IDbConnectionFactory connectionFactory) : IRegionAccessService
{
    public async Task<IReadOnlyList<RegionOption>> GetAvailableRegionsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Array.Empty<RegionOption>();
        }

        const string sql = """
            SELECT
                r.region_code AS RegionCode,
                r.region_name AS RegionName,
                r.address AS Address,
                TRUE AS CanAccess
            FROM user_regions ur
            INNER JOIN regions r ON r.region_code = ur.region_code
            WHERE ur.user_id = @UserId
            ORDER BY r.region_name;
            """;

        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken);
        var regions = await connection.QueryAsync<RegionOption>(command);

        return regions.ToList();
    }
}
