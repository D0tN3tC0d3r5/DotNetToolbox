namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal class TestGuidProvider()
    : IGuidProvider {
    public Guid New() => throw new NotImplementedException();
    public Guid New(byte[] bytes) => throw new NotImplementedException();
    public Guid New(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        => throw new NotImplementedException();
    public Guid New(int a, short b, short c, byte[] d) => throw new NotImplementedException();
    public Guid New(ReadOnlySpan<byte> bytes) => throw new NotImplementedException();
    public Guid New(ReadOnlySpan<byte> bytes, bool bigEndian) => throw new NotImplementedException();
    public Guid New(string text) => throw new NotImplementedException();
    public Guid New(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) 
        => throw new NotImplementedException();

    public Guid Parse(ReadOnlySpan<char> input) => throw new NotImplementedException();
    public Guid Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
    public Guid Parse(string input) => throw new NotImplementedException();
    public Guid Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();

    public Guid ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format) => throw new NotImplementedException();
    public Guid ParseExact(string input, string format) => throw new NotImplementedException();

    public bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Guid result) => throw new NotImplementedException();
    public bool TryParse(ReadOnlySpan<char> input, out Guid result) => throw new NotImplementedException();
    public bool TryParse(string? s, IFormatProvider? provider, out Guid result) => throw new NotImplementedException();
    public bool TryParse(string? input, out Guid result) => throw new NotImplementedException();

    public bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out Guid result) => throw new NotImplementedException();
    public bool TryParseExact(string? input, string? format, out Guid result) => throw new NotImplementedException();
}
