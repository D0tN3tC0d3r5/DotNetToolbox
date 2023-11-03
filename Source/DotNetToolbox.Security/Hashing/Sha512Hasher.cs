namespace DotNetToolbox.Security.Hashing;

public sealed class Sha512Hasher
    : IHasher {
    public const int DefaultIterations = 259733; // Some not exact number

    private const int _saltSize = 64; // 512 bits
    private readonly int _iterations;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public Sha512Hasher() {
        _iterations = DefaultIterations;
    }

    public Sha512Hasher(int iterations) {
        if (iterations < 1)
            throw new ArgumentOutOfRangeException(nameof(iterations), iterations, "Iterations must be greater than 0.");
        _iterations = iterations;
    }

    public Hash Generate(string input) => Generate(Encoding.UTF8.GetBytes(IsNotNull(input)));

    public Hash Generate(byte[] input) {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var hash = ExecuteHashing(input, salt);
        return new(hash, salt);
    }

    public bool Validate(Hash secret, string input) => Validate(IsNotNull(secret), Encoding.UTF8.GetBytes(IsNotNull(input)));

    public bool Validate(Hash secret, byte[] input) {
        var hash = ExecuteHashing(IsNotNull(input), IsNotNull(secret).Salt);
        return secret.Value.SequenceEqual(hash);
    }
    private byte[] ExecuteHashing(byte[] input, byte[] salt)
        => Rfc2898DeriveBytes.Pbkdf2(input,
                                     salt,
                                     _iterations,
                                     _hashAlgorithm,
                                     _saltSize);
}
