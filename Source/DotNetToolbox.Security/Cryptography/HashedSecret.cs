namespace DotNetToolbox.Security.Cryptography;

public sealed record HashedSecret {
    public HashedSecret() {
    }

    public HashedSecret(string hash, string salt)
        : this() {
        Hash = Ensure.IsNotNullOrWhiteSpace(hash);
        Salt = Ensure.IsNotNullOrWhiteSpace(salt);
        HashBytes = Convert.FromBase64String(Hash);
        SaltBytes = Convert.FromBase64String(Salt);
    }

    public HashedSecret(byte[] hashBytes, byte[] saltBytes)
        : this() {
        HashBytes = Ensure.IsNotNullOrEmpty(hashBytes);
        SaltBytes = Ensure.IsNotNullOrEmpty(saltBytes);
        Hash = Convert.ToBase64String(HashBytes);
        Salt = Convert.ToBase64String(SaltBytes);
    }

    public string Hash { get; init; } = string.Empty;
    public string Salt { get; init; } = string.Empty;

    public byte[] HashBytes { get; init; } = Array.Empty<byte>();
    public byte[] SaltBytes { get; init; } = Array.Empty<byte>();

    public bool Verify(string secret, IHasher hasher) {
        var hashedSecret = hasher.HashSecret(secret, SaltBytes);
        return hashedSecret.Hash == Hash;
    }
}