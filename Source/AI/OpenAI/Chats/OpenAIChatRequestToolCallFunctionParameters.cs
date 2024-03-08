namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestToolCallFunctionParameters(Dictionary<string, OpenAIChatRequestToolCallFunctionParameter>? properties, string[]? required) {
    [JsonPropertyName("properties")]
    public Dictionary<string, OpenAIChatRequestToolCallFunctionParameter>? Properties { get; set; } = properties;
    [JsonPropertyName("required")]
    public string[]? Required { get; set; } = required;
}
