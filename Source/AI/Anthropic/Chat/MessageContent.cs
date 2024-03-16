namespace DotNetToolbox.AI.Anthropic.Chat;

public class MessageContent {
    public MessageContent(object value) {
        Text = value as string;
        Image = value as ImageData;
        Type = Text is null ? "image" : "text";
    }

    [JsonPropertyName("type")]
    public string Type { get; init; }
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    [JsonPropertyName("image")]
    public ImageData? Image { get; init; }
}
