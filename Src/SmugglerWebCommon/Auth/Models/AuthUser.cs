namespace SmugglerWebCommon.Auth.Models;

public sealed class AuthUser
{
    public string UserId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; } = UserRole.None;
    public IReadOnlyCollection<string> Regions { get; init; } = Array.Empty<string>();
}
