namespace System.Security.Cryptography;

public interface IHasher {
    HashedSecret HashSecret(string secret, byte[]? saltBytes = null);
}