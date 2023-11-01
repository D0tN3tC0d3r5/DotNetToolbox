namespace DotNetToolbox.Security.Hashing;

public class Sha512Hasher(int keySize = Sha512Hasher.DefaultKeySize, int iterations = Sha512Hasher.DefaultIterations)
    : IHasher {
    public const int DefaultKeySize = 64;
    public const int DefaultIterations = 350000;

    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public Hash Generate(byte[] secret) {
        var salt = RandomNumberGenerator.GetBytes(keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            IsNotNull(secret),
            salt,
            iterations,
            _hashAlgorithm,
            keySize);
        return new(hash, salt);
    }

    public bool Validate(Hash hash, byte[] secret) {
        var testValue = Rfc2898DeriveBytes.Pbkdf2(
                                         IsNotNull(secret),
                                         hash.Salt,
                                         iterations,
                                         _hashAlgorithm,
                                         keySize);
        return hash.Value.SequenceEqual(testValue);
    }
}
