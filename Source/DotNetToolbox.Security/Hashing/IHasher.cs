namespace DotNetToolbox.Security.Hashing;

public interface IHasher {
    Hash Generate(byte[] secret);
    Hash Generate(string secret);
    bool Validate(Hash hash, byte[] secret);
    bool Validate(Hash hash, string secret);
}
