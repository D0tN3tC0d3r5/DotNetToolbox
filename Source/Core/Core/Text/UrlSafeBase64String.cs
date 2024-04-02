// ReSharper disable once CheckNamespace
namespace System.Text;

public readonly partial record struct UrlSafeBase64String {
    public UrlSafeBase64String() {
        Bytes = [];
    }

    public UrlSafeBase64String(byte[]? input = null) {
        Bytes = input ?? [];
    }

    public UrlSafeBase64String(Guid input) {
        Bytes = input.ToByteArray();
    }

    public UrlSafeBase64String(string input) {
        input = input?.Trim() ?? string.Empty;
        Bytes = _safeBase64String.IsMatch(input)
            ? ToBytes(input)
            : Encoding.UTF8.GetBytes(input);
    }

    public byte[] Bytes { get; }
    public string Text => Encoding.UTF8.GetString(Bytes);
    public string Base64 => ToBase64(Bytes);
    public bool IsGuid => Bytes.Length is 0 or 16;
    public Guid Guid => Bytes.Length switch {
        0 => Guid.Empty,
        16 => new(Bytes),
        _ => throw new FormatException("The value is not a valid GUID."),
    };

    public static implicit operator UrlSafeBase64String(byte[]? input) => new(input);
    public static implicit operator UrlSafeBase64String(string input) => new(input);
    public static implicit operator UrlSafeBase64String(Guid input) => new(input);
    public static implicit operator byte[](UrlSafeBase64String input) => input.Bytes;
    public static implicit operator string(UrlSafeBase64String input) => input.Base64;
    public static implicit operator Guid(UrlSafeBase64String input) => input.Guid;

    private static string ToBase64(byte[] bytes)
        => bytes.Length == 0
        ? string.Empty
        : ToSafeBase64(bytes);

    private static string ToSafeBase64(byte[] bytes) {
        var base64 = Convert.ToBase64String(bytes);
        var builder = new StringBuilder(base64.TrimEnd('='));
        builder.Replace('+', '-');
        builder.Replace('/', '_');
        return builder.ToString();
    }

    private static byte[] ToBytes(string input)
        => Convert.FromBase64String(ToStandardBase64(input));

    private static string ToStandardBase64(string input) {
        var builder = new StringBuilder(input.Trim());
        builder.Replace('_', '/');
        builder.Replace('-', '+');
        builder.Append('=', 24 - (input.Length % 24));
        return builder.ToString();
    }

    private static readonly Regex _safeBase64String = SafeBase64String();
    [GeneratedRegex("^[a-zA-Z0-9_-]{22}$", RegexOptions.Compiled)]
    private static partial Regex SafeBase64String();
}
