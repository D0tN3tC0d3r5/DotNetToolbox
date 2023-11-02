namespace DotNetToolbox.Security.Hashing;

public class Hash {

    public Hash(byte[] value, byte[] salt) {
        Value = IsNotNull(value);
        Salt = IsNotNull(salt);
    }

    public byte[] Value { get; }
    public byte[] Salt { get; }
}