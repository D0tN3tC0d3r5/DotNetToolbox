namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatRequestResponseFormat(string type) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
}
