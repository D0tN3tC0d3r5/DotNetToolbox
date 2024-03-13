namespace DotNetToolbox.AI.OpenAI.Chat;

public class RequestToolFunctionCall(string name, ChatRequestToolFunctionCallParameters? parameters = null, string? description = null) {
    [JsonPropertyName("name")]
    public string Name { get; set; } = name;
    [JsonPropertyName("parameters")]
    public ChatRequestToolFunctionCallParameters? Parameters { get; set; } = parameters;
    [JsonPropertyName("description")]
    public string? Description { get; set; } = description;
}
