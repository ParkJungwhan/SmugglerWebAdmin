using System.Security.Claims;
using SmugglerWebCommon.Auth.Models;

namespace SmugglerSelectWeb.Services.Auth;

public interface IRegionAccessService
{
    Task<IReadOnlyList<RegionOption>> GetAvailableRegionsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
}

public sealed class InMemoryRegionAccessService : IRegionAccessService
{
    public Task<IReadOnlyList<RegionOption>> GetAvailableRegionsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        var isAdmin = user.IsInRole("Admin") || user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));

        IReadOnlyList<RegionOption> regions =
        [
            new RegionOption { RegionCode = "KR-SEOUL", RegionName = "Seoul", CanAccess = true },
            new RegionOption { RegionCode = "JP-TOKYO", RegionName = "Tokyo", CanAccess = isAdmin },
            new RegionOption { RegionCode = "US-EAST", RegionName = "US East", CanAccess = isAdmin }
        ];

        return Task.FromResult(regions);
    }
}
