namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestToolCallFunction(string name, OpenAIChatRequestToolCallFunctionParameters? parameters = null, string? description = null) {
    [JsonPropertyName("name")]
    public string Name { get; set; } = name;
    [JsonPropertyName("parameters")]
    public OpenAIChatRequestToolCallFunctionParameters? Parameters { get; set; } = parameters;
    [JsonPropertyName("description")]
    public string? Description { get; set; } = description;
}
