namespace DotNetToolbox.Security.Encryption;
public interface IEncoder : IDisposable {
    byte[] Encode(byte[] input);
    string Encode(string input);
    byte[] Decode(byte[] secret);
    string Decode(string secret);
}
