using System.Diagnostics.CodeAnalysis;

namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal sealed class TestStringGuidProvider
    : IStringGuidProvider {
    public string Create() => throw new NotImplementedException();
    public string Create(byte[] bytes) => throw new NotImplementedException();
    public string Create(ReadOnlySpan<byte> bytes) => throw new NotImplementedException();
    public string CreateSortable() => throw new NotImplementedException();
    public string CreateSortable(byte[] bytes) => throw new NotImplementedException();
    public string CreateSortable(ReadOnlySpan<byte> bytes) => throw new NotImplementedException();

    public string EnsureIsValid(ReadOnlySpan<char> input) => throw new NotImplementedException();
    public string EnsureIsValid(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
    public string EnsureIsValid(string input) => throw new NotImplementedException();
    public string EnsureIsValid(string s, IFormatProvider? provider) => throw new NotImplementedException();
    public string EnsureIsValid(ReadOnlySpan<char> input, ReadOnlySpan<char> format) => throw new NotImplementedException();
    public string EnsureIsValid(string input, string format) => throw new NotImplementedException();

    public bool IsValid(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
    public bool IsValid(ReadOnlySpan<char> input) => throw new NotImplementedException();
    public bool IsValid([NotNullWhen(true)] string? s, IFormatProvider? provider) => throw new NotImplementedException();
    public bool IsValid([NotNullWhen(true)] string? input) => throw new NotImplementedException();
    public bool IsValid(ReadOnlySpan<char> input, ReadOnlySpan<char> format) => throw new NotImplementedException();
    public bool IsValid([NotNullWhen(true)] string? input, [NotNullWhen(true)] string? format) => throw new NotImplementedException();
}
