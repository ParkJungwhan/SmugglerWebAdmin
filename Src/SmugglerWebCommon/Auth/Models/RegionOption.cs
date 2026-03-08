namespace SmugglerWebCommon.Auth.Models;

public sealed class RegionOption
{
    public string RegionCode { get; init; } = string.Empty;
    public string RegionName { get; init; } = string.Empty;
    public bool CanAccess { get; init; }
}
