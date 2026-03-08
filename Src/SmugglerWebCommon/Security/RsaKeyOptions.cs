namespace SmugglerWebCommon.Security;

public sealed class RsaKeyOptions
{
    public string PublicKeyPem { get; init; } = string.Empty;
    public string PrivateKeyPem { get; init; } = string.Empty;
}
