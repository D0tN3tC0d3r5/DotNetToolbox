namespace DotNetToolbox.Security.Hashing;

public class Hash(byte[] value, byte[] salt) {
    public byte[] Value { get; } = IsNotNull(value);
    public byte[] Salt { get; } = IsNotNull(salt);
}
