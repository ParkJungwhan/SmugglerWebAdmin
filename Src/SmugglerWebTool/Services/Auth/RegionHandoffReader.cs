using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using SmugglerWebCommon.Auth.Models;

namespace SmugglerWebTool.Services.Auth;

public interface IRegionHandoffReader
{
    bool TryRead(string token, out RegionAccessContext? context);
}

public sealed class RegionHandoffReader(IDataProtectionProvider dataProtectionProvider) : IRegionHandoffReader
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("Smuggler.Region.Handoff.v1");

    public bool TryRead(string token, out RegionAccessContext? context)
    {
        context = null;

        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        try
        {
            var protectedBytes = Convert.FromBase64String(token);
            var payload = _protector.Unprotect(protectedBytes);
            var json = Encoding.UTF8.GetString(payload);
            var parsed = JsonSerializer.Deserialize<RegionAccessContext>(json);

            if (parsed is null || parsed.ExpiresAtUtc < DateTimeOffset.UtcNow)
            {
                return false;
            }

            context = parsed;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
