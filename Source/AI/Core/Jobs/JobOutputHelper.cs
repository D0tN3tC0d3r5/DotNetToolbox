namespace DotNetToolbox.AI.Jobs;

internal static class JobOutputHelper {
    private static readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNameCaseInsensitive = true,
        Converters = { new JobObjectSerializer() },
    };

    public static object ExtractOutput(TaskResponseType responseType, Message response) {
        var responseText = response.ToString();
        return responseType switch {
            TaskResponseType.Json => ConvertToDictionary(responseText),
            TaskResponseType.List => ConvertToList(responseText),
            TaskResponseType.Table => ConvertToTable(responseText),
            _ => responseText,
        };
    }

    private static List<List<string>> ConvertToTable(string responseText)
        => responseText.Split('\n').ToList(line => line.Split('|').ToList(cell => cell));

    private static List<object> ConvertToList(string responseText)
        => JsonSerializer.Deserialize<List<object>>(responseText, _jsonOptions) ?? [];

    private static Map ConvertToDictionary(string responseText) {
        var result = JsonSerializer.Deserialize<Map>(responseText, _jsonOptions);
        return result ?? [];
    }
}
