namespace DotNetToolbox.AI.Jobs;

internal static class JobOutputHelper {

    private static readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNameCaseInsensitive = true,
        Converters = { new JobOutputConverter() }
    };

    public static object ExtractOutput(TaskResponseType responseType, Message response) {
        var responseText = response.ToString();
        return response.Type switch {
            TaskResponseType.Json => ConvertToMap(responseText),
            TaskResponseType.Table => ConvertToTable(responseText),
            _ => responseText,
        };
    }
    private static string[][] ConvertToTable(string responseText) => responseText.Split('\n').Select(line => line.Split('|').Select(cell => cell.Trim()).ToArray()).ToArray();
    private static Dictionary<string, object> ConvertToMap(string responseText) => JsonSerializer.Deserialize<Dictionary<string, object>>(responseText, _jsonOptions) ?? [];
}

