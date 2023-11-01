namespace DotNetToolbox.Security.Hashing;

public readonly record struct Hash {

    // ReSharper disable once ConvertToPrimaryConstructor - no setters
    public Hash(byte[] value, byte[] salt) {
        Value = value;
        Salt = salt;
    }

    public byte[] Value { get; }
    public byte[] Salt { get; }
}