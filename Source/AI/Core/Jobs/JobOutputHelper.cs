namespace DotNetToolbox.AI.Jobs;

internal static class JobOutputHelper {
    private static readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNameCaseInsensitive = true,
        Converters = { new JobObjectSerializer() },
    };

    public static object ExtractOutput(TaskResponseType responseType, Message response) {
        var responseText = response.ToString();
        return responseType switch {
            TaskResponseType.Json => ConvertToMap(responseText),
            TaskResponseType.List => ConvertToList(responseText),
            TaskResponseType.Table => ConvertToTable(responseText),
            _ => responseText,
        };
    }

    private static string[][] ConvertToTable(string responseText)
        => responseText.Split('\n').Select(line => line.Split('|').Select(cell => cell.Trim()).ToArray()).ToArray();

    private static List<object> ConvertToList(string responseText)
        => JsonSerializer.Deserialize<List<object>>(responseText, _jsonOptions) ?? [];

    private static Dictionary<string, object> ConvertToMap(string responseText)
        => JsonSerializer.Deserialize<Dictionary<string, object>>(responseText, _jsonOptions) ?? [];
}
