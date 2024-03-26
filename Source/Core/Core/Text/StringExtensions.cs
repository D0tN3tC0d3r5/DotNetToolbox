namespace DotNetToolbox.Text;

public static class StringExtensions {
    public static string ToBase64(this string value)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

    public static string ToSafeBase64(this string value)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
}
