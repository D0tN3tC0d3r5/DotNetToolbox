namespace DotNetToolbox.Security.Hashing;

public class Sha512Hasher : IHasher {
    public const int DefaultKeySize = 64;
    public const int DefaultIterations = 350000;

    private readonly int _keySize;
    private readonly int _iterations;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public Sha512Hasher(int keySize = DefaultKeySize, int iterations = DefaultIterations) {
        _keySize = keySize;
        _iterations = iterations;
    }

    public Hash Generate(string secret) => Generate(Encoding.UTF8.GetBytes(IsNotNull(secret)));

    public Hash Generate(byte[] secret) {
        var salt = RandomNumberGenerator.GetBytes(_keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            IsNotNull(secret),
            salt,
            _iterations,
            _hashAlgorithm,
            _keySize);
        return new(hash, salt);
    }

    public bool Validate(Hash hash, string secret) => Validate(IsNotNull(hash), Encoding.UTF8.GetBytes(IsNotNull(secret)));

    public bool Validate(Hash hash, byte[] secret) {
        var testValue = Rfc2898DeriveBytes.Pbkdf2(
                                         IsNotNull(secret),
                                         hash.Salt,
                                         _iterations,
                                         _hashAlgorithm,
                                         _keySize);
        return IsNotNull(hash).Value.SequenceEqual(testValue);
    }
}
