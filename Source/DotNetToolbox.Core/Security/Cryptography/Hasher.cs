namespace System.Security.Cryptography;

public class Hasher : IHasher {
    private const int _keySize = 64;
    private const int _iterations = 350000;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public HashedSecret HashSecret(string secret, byte[]? saltBytes = null) {
        var secretBytes = Encoding.UTF8.GetBytes(Ensure.IsNotNullOrWhiteSpace(secret));
        saltBytes ??= RandomNumberGenerator.GetBytes(_keySize);
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            secretBytes,
            saltBytes,
            _iterations,
            _hashAlgorithm,
            _keySize);

        return new HashedSecret(hashBytes, saltBytes);
    }
}
