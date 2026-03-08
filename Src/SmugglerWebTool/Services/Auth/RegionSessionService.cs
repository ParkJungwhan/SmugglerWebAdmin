using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using SmugglerWebCommon.Auth.Models;

namespace SmugglerWebTool.Services.Auth;

public interface IRegionSessionService
{
    void Set(HttpContext httpContext, RegionAccessContext context);
    bool TryGet(HttpContext httpContext, out RegionAccessContext? context);
    void Clear(HttpContext httpContext);
}

public sealed class RegionSessionService(IDataProtectionProvider dataProtectionProvider) : IRegionSessionService
{
    private const string CookieName = "smuggler_region_ctx";
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("Smuggler.Region.Session.v1");

    public void Set(HttpContext httpContext, RegionAccessContext context)
    {
        var json = JsonSerializer.Serialize(context);
        var payload = Encoding.UTF8.GetBytes(json);
        var protectedPayload = _protector.Protect(payload);

        httpContext.Response.Cookies.Append(
            CookieName,
            Convert.ToBase64String(protectedPayload),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = context.ExpiresAtUtc,
                IsEssential = true
            });
    }

    public bool TryGet(HttpContext httpContext, out RegionAccessContext? context)
    {
        context = null;

        if (!httpContext.Request.Cookies.TryGetValue(CookieName, out var cookieValue) || string.IsNullOrWhiteSpace(cookieValue))
        {
            return false;
        }

        try
        {
            var protectedPayload = Convert.FromBase64String(cookieValue);
            var payload = _protector.Unprotect(protectedPayload);
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

    public void Clear(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(CookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        });
    }
}
