namespace Sophia.Data.Helpers;

public class StringArrayConversion {
    internal static string[] ConvertToArray(string? s)
        => s == null ? [] : s.Split('|');

    internal static string? ConvertToString(string[] choices)
        => choices.Length == 0 ? null : string.Join('|', choices);
}
