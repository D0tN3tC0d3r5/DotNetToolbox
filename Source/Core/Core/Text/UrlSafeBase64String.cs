// ReSharper disable once CheckNamespace
namespace System.Text;

public readonly partial record struct UrlSafeBase64String {
    public UrlSafeBase64String(byte[]? input = null) {
        Bytes = input ?? [];
    }

    public UrlSafeBase64String(string? input) {
        input = input?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(input)) {
            Bytes = [];
            return;
        }
        if (_safeBase64Format.IsMatch(input)) {
            Bytes = UrlSafeBase64Converter.GetBytes(input);
            return;
        }

        var result = new byte[input.Length * 2];
        if (Convert.TryFromBase64String(input, result.AsSpan(), out var size)) {
            Array.Resize(ref result, size);
            Bytes = result;
            return;
        }

        Bytes = Encoding.UTF8.GetBytes(input);
    }

    public byte[] Bytes { get; }
    public string PlainText => Encoding.UTF8.GetString(Bytes);
    public string Encoded => UrlSafeBase64Converter.GetString(Bytes);

    public static implicit operator UrlSafeBase64String(byte[]? input) => new(input);
    public static implicit operator UrlSafeBase64String(string input) => new(input);
    public static implicit operator byte[](UrlSafeBase64String input) => input.Bytes;
    public static implicit operator string(UrlSafeBase64String input) => input.Encoded;

    private static readonly Regex _safeBase64Format = SafeBase64Format();
    [GeneratedRegex("^[a-zA-Z0-9_-]{22}$", RegexOptions.Compiled)]
    private static partial Regex SafeBase64Format();
}
