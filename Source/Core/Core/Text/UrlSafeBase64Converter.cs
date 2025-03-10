namespace System.Text;

public static class UrlSafeBase64Converter {
    public static string GetString(byte[] input)
        => Convert.ToBase64String(input)
                  .Replace('+', '-')
                  .Replace('/', '_');

    public static byte[] GetBytes(string input)
        => TryGetBytes(input, out var bytes)
               ? bytes
               : throw new InvalidOperationException("The input is not a valid Url safe Base64 string.");

    public static bool TryGetBytes(string input, out byte[] bytes) {
        bytes = [];
        if (string.IsNullOrEmpty(input)) return true;
        var builder = new StringBuilder(input)
                     .Replace('-', '+')
                     .Replace('_', '/');
        while (builder.Length % 4 != 0) builder.Append('=');
        var result = new byte[input.Length * 2];
        if (!Convert.TryFromBase64String(input, result.AsSpan(), out var size)) return false;
        Array.Resize(ref result, size);
        bytes = result;
        return true;
    }
}
