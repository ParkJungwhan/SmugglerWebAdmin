namespace SmugglerWebCommon.Auth.Models;

public sealed class RegionOption
{
    public int RegionCode { get; init; }
    public string RegionName { get; init; } = string.Empty;
    public bool CanAccess { get; init; }
}
