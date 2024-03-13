namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatRequestResponseFormat(string type) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
}
