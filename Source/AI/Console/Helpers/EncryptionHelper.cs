namespace AI.Sample.Helpers;

public interface IEncryptionHelper {
    string? Encrypt(string? plainText);
    string? Decrypt(string? cipherText);
}

public sealed class EncryptionHelper : IEncryptionHelper {
    private static IEncryptionHelper? _instance;
    private readonly byte[] _secretKey;
    private readonly byte[] _initVector;

    private EncryptionHelper(IConfiguration configuration) {
        _secretKey = Convert.FromBase64String(IsNotNull(configuration["Encryption:SecretKey"]));
        _initVector = Convert.FromBase64String(IsNotNull(configuration["Encryption:InitVector"]));
    }

    public static void Initialize(IConfiguration configuration) {
        if (_instance != null) return;
        _instance = new EncryptionHelper(configuration);
    }

    public static IEncryptionHelper Instance
        => _instance
        ?? throw new InvalidOperationException("EncryptionHelper has not been initialized.");

    public string? Encrypt(string? plainText) {
        if (plainText is null) return null;
        if (plainText.Length == 0) return string.Empty;
        using var aesAlg = Aes.Create();
        aesAlg.Key = _secretKey;
        aesAlg.IV = _initVector;

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using var swEncrypt = new StreamWriter(csEncrypt);
        swEncrypt.Write(plainText);

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public string? Decrypt(string? cipherText) {
        try {
            if (cipherText is null) return null;
            if (cipherText.Length == 0) return string.Empty;
            var cipherBytes = Convert.FromBase64String(cipherText);

            using var aesAlg = Aes.Create();
            aesAlg.Key = _secretKey;
            aesAlg.IV = _initVector;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(cipherBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
        catch (FormatException) {
            return null;
        }
    }
}
