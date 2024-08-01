namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatRequestResponseFormat(string type) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
}
