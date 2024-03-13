namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatResponseFunctionCallRequest(string name, string? arguments) {
    [JsonPropertyName("name")]
    public string Name { get; set; } = name;

    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; } = arguments;
}
