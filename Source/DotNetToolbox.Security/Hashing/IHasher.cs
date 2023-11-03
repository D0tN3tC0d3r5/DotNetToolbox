namespace DotNetToolbox.Security.Hashing;

public interface IHasher {
    Hash Generate(byte[] input);
    Hash Generate(string input);
    bool Validate(Hash secret, byte[] input);
    bool Validate(Hash secret, string input);
}
