namespace SmugglerWebCommon.Auth.Models;

public sealed class RegionAccessContext
{
    public string UserId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int RegionCode { get; init; }
    public string Role { get; init; } = string.Empty;
    public DateTimeOffset IssuedAtUtc { get; init; }
    public DateTimeOffset ExpiresAtUtc { get; init; }
}
