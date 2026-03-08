using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using SmugglerWebCommon.Auth.Models;

namespace SmugglerSelectWeb.Services.Auth;

public interface IRegionHandoffTokenService
{
    string CreateToken(ClaimsPrincipal user, int regionCode, TimeSpan lifetime);
}

public sealed class RegionHandoffTokenService(IDataProtectionProvider dataProtectionProvider) : IRegionHandoffTokenService
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("Smuggler.Region.Handoff.v1");

    public string CreateToken(ClaimsPrincipal user, int regionCode, TimeSpan lifetime)
    {
        var now = DateTimeOffset.UtcNow;
        var context = new RegionAccessContext
        {
            UserId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            Email = user.FindFirstValue(ClaimTypes.Email) ?? user.Identity?.Name ?? string.Empty,
            RegionCode = regionCode,
            Role = user.FindFirstValue(ClaimTypes.Role) ?? "Viewer",
            IssuedAtUtc = now,
            ExpiresAtUtc = now.Add(lifetime)
        };

        var json = JsonSerializer.Serialize(context);
        var payload = Encoding.UTF8.GetBytes(json);
        var protectedPayload = _protector.Protect(payload);

        return Convert.ToBase64String(protectedPayload);
    }
}
