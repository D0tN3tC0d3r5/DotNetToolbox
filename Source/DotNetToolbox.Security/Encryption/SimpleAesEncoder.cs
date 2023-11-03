namespace DotNetToolbox.Security.Encryption;

public sealed class SimpleAesEncoder : IEncoder {
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public SimpleAesEncoder(byte[] key, byte[]? iv = null) {
        _key = IsNotNull(key);
        if (_key.Length != 32) throw new ArgumentException("Encryption key must be 32 bytes long.", nameof(iv));
        _iv = iv ?? new byte[16];
        if (_iv.Length != 16) throw new ArgumentException("Initialization vector must be 16 bytes long.", nameof(iv));
    }

    private bool _isDisposed;
    public void Dispose() {
        if (_isDisposed) return;
        Array.Clear(_key);
        Array.Clear(_iv);
        _isDisposed = true;
    }

    public byte[] Encode(byte[] input)
        => ExecuteEncoding(Encoding.UTF8.GetString(IsNotNull(input)));

    public string Encode(string input)
        => Convert.ToBase64String(ExecuteEncoding(IsNotNull(input)));

    public byte[] Decode(byte[] secret)
        => Encoding.UTF8.GetBytes(ExecuteDecoding(IsNotNull(secret)));

    public string Decode(string secret)
        => ExecuteDecoding(Convert.FromBase64String(IsNotNull(secret)));

    private byte[] ExecuteEncoding(string input) {
        if (_isDisposed) throw new InvalidOperationException("This resource has already been disposed.");
        using var aes = Aes.Create();
        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(_key, _iv), CryptoStreamMode.Write)) {
            using var streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(input);
        }
        return memoryStream.ToArray();
    }

    private string ExecuteDecoding(byte[] secret) {
        if (_isDisposed) throw new InvalidOperationException("This resource has already been disposed.");
        using var memoryStream = new MemoryStream(secret);
        using var aes = Aes.Create();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(_key, _iv), CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }
}
