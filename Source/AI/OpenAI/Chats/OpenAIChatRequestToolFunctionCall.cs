namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestToolFunctionCall(string name, OpenAIChatRequestToolFunctionCallParameters? parameters = null, string? description = null) {
    [JsonPropertyName("name")]
    public string Name { get; set; } = name;
    [JsonPropertyName("parameters")]
    public OpenAIChatRequestToolFunctionCallParameters? Parameters { get; set; } = parameters;
    [JsonPropertyName("description")]
    public string? Description { get; set; } = description;
}
