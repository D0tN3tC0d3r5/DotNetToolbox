namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestToolFunctionCallParameters(Dictionary<string, OpenAIChatRequestToolFunctionCallParameter>? properties, string[]? required) {
    [JsonPropertyName("properties")]
    public Dictionary<string, OpenAIChatRequestToolFunctionCallParameter>? Properties { get; set; } = properties;
    [JsonPropertyName("required")]
    public string[]? Required { get; set; } = required;
}
