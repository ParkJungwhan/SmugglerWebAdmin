using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using SmugglerWebCommon.Auth.Models;

namespace SmugglerSelectWeb.Services.Auth;

public interface IRegionHandoffTokenService
{
    string CreateToken(string userId, string email, string role, int regionCode, TimeSpan lifetime);
}

public sealed class RegionHandoffTokenService(IDataProtectionProvider dataProtectionProvider) : IRegionHandoffTokenService
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("Smuggler.Region.Handoff.v1");

    public string CreateToken(string userId, string email, string role, int regionCode, TimeSpan lifetime)
    {
        var now = DateTimeOffset.UtcNow;
        var context = new RegionAccessContext
        {
            UserId = userId,
            Email = email,
            RegionCode = regionCode,
            Role = role,
            IssuedAtUtc = now,
            ExpiresAtUtc = now.Add(lifetime)
        };

        var json = JsonSerializer.Serialize(context);
        var payload = Encoding.UTF8.GetBytes(json);
        var protectedPayload = _protector.Protect(payload);

        return Convert.ToBase64String(protectedPayload);
    }
}
