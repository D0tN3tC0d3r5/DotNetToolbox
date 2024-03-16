namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatRequestForceToolCall(string name) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = "function";
    [JsonPropertyName("name")]
    public string Name { get; set; } = name;
}
