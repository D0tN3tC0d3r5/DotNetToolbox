namespace DotNetToolbox.Security.Hashing;

public interface IHasher {
    Hash Generate(byte[] secret);
    bool Validate(Hash hash, byte[] secret);
}