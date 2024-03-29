﻿namespace DotNetToolbox;

public interface IGuidProvider {
    Guid New();
    Guid New(byte[] bytes);
    Guid New(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k);
    Guid New(int a, short b, short c, byte[] d);
    Guid New(ReadOnlySpan<byte> bytes);
    Guid New(ReadOnlySpan<byte> bytes, bool bigEndian);
    Guid New(string text);
    Guid New(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k);
    Guid Parse(ReadOnlySpan<char> input);
    Guid Parse(ReadOnlySpan<char> s, IFormatProvider? provider);
    Guid Parse(string input);
    Guid Parse(string s, IFormatProvider? provider);
    Guid ParseExact(ReadOnlySpan<char> input, [Syntax("GuidFormat")] ReadOnlySpan<char> format);
    Guid ParseExact(string input, [Syntax("GuidFormat")] string format);
    bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Guid result);
    bool TryParse(ReadOnlySpan<char> input, out Guid result);
    bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Guid result);
    bool TryParse([NotNullWhen(true)] string? input, out Guid result);
    bool TryParseExact(ReadOnlySpan<char> input, [Syntax("GuidFormat")] ReadOnlySpan<char> format, out Guid result);
    bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true), Syntax("GuidFormat")] string? format, out Guid result);
}

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class GuidProvider : HasDefault<GuidProvider>, IGuidProvider {
    public virtual Guid New() => Guid.NewGuid();
    public virtual Guid New(string text) => new(text);
    public virtual Guid New(byte[] bytes) => new(bytes);
    public virtual Guid New(ReadOnlySpan<byte> bytes) => new(bytes);
    public virtual Guid New(ReadOnlySpan<byte> bytes, bool bigEndian) => new(bytes, bigEndian);
    public virtual Guid New(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        => new(a, b, c, d, e, f, g, h, i, j, k);
    public virtual Guid New(int a, short b, short c, byte[] d)
        => new(a, b, c, d);
    public virtual Guid New(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        => new(a, b, c, d, e, f, g, h, i, j, k);

    public virtual Guid Parse(string input) => Guid.Parse(input);
    public virtual Guid Parse(ReadOnlySpan<char> input) => Guid.Parse(input);
    public virtual Guid Parse(string s, IFormatProvider? provider) => Guid.Parse(s, provider);
    public virtual Guid Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Guid.Parse(s, provider);

    public virtual Guid ParseExact(string input, [Syntax(Syntax.GuidFormat)] string format)
        => Guid.ParseExact(input, format);
    public virtual Guid ParseExact(ReadOnlySpan<char> input, [Syntax(Syntax.GuidFormat)] ReadOnlySpan<char> format)
        => Guid.ParseExact(input, format);

    public virtual bool TryParse([NotNullWhen(true)] string? input, out Guid result)
        => Guid.TryParse(input, out result);
    public virtual bool TryParse(ReadOnlySpan<char> input, out Guid result)
        => Guid.TryParse(input, out result);
    public virtual bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Guid result)
        => Guid.TryParse(s, provider, out result);
    public virtual bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Guid result)
        => Guid.TryParse(s, provider, out result);

    public virtual bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true), Syntax(Syntax.GuidFormat)] string? format, out Guid result)
        => Guid.TryParseExact(input, format, out result);
    public virtual bool TryParseExact(ReadOnlySpan<char> input, [Syntax(Syntax.GuidFormat)] ReadOnlySpan<char> format, out Guid result)
        => Guid.TryParseExact(input, format, out result);
}
