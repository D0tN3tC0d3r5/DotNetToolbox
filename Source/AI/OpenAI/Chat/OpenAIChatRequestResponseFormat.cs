namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestResponseFormat(string type) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
}
