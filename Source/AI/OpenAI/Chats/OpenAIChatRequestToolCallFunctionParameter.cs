namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestToolCallFunctionParameter(string type, string[]? options = null, string? description = null) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("description")]
    public string? Description { get; set; } = description;
    [JsonPropertyName("enum")]
    public string[]? Options { get; set; } = options;
}
