namespace System.Text;

public static class UrlSafeBase64Converter {
    public static string GetString(byte[] input)
        => Convert.ToBase64String(input)
                  .Replace('+', '-')
                  .Replace('/', '_')
                  .Replace("=", "");

    public static byte[] GetBytes(string input)
        => TryGetBytes(input, out var bytes)
               ? bytes
               : throw new InvalidOperationException("The input is not a valid Url safe Base64 string.");

    public static bool TryGetBytes(string input, out byte[] bytes) {
        bytes = [];
        if (string.IsNullOrEmpty(input)) return true;
        try {
            var builder = new StringBuilder(input)
                         .Replace('-', '+')
                         .Replace('_', '/');
            while (builder.Length % 4 != 0) builder.Append('=');
            bytes = Convert.FromBase64String(builder.ToString());
            Base64.EncodeToUtf8InPlace(bytes, bytes.Length, out _);
            return true;
        }
        catch {
            return false;
        }
    }
}
