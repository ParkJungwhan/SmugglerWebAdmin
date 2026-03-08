using System.Security.Cryptography;
using System.Text;

namespace SmugglerWebCommon.Security;

public interface IRsaCryptoService
{
    string EncryptWithPublicKey(string plainText);

    string DecryptWithPrivateKey(string cipherTextBase64);
}

public sealed class RsaCryptoService : IRsaCryptoService
{
    private readonly string _publicKeyPem;
    private readonly string _privateKeyPem;

    public RsaCryptoService(RsaKeyOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.PublicKeyPem) && !string.IsNullOrWhiteSpace(options.PrivateKeyPem))
        {
            _publicKeyPem = options.PublicKeyPem;
            _privateKeyPem = options.PrivateKeyPem;
            return;
        }

        using var rsa = RSA.Create(2048);
        _publicKeyPem = rsa.ExportRSAPublicKeyPem();
        _privateKeyPem = rsa.ExportRSAPrivateKeyPem();
    }

    public string EncryptWithPublicKey(string plainText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plainText);

        using var rsa = RSA.Create();
        rsa.ImportFromPem(_publicKeyPem.AsSpan());

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encrypted = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(encrypted);
    }

    public string DecryptWithPrivateKey(string cipherTextBase64)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cipherTextBase64);

        using var rsa = RSA.Create();
        rsa.ImportFromPem(_privateKeyPem.AsSpan());

        var encryptedBytes = Convert.FromBase64String(cipherTextBase64);
        var decrypted = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decrypted);
    }
}