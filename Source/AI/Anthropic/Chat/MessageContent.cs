namespace DotNetToolbox.AI.Anthropic.Chat;

public class MessageContent() {
    [SetsRequiredMembers]
    public MessageContent(object value)
        : this() {
        Text = value as string;
        Image = value as ImageData;
        Type = Text is null ? "image" : "text";
    }

    [JsonPropertyName("type")]
    public required string Type { get; init; }
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    [JsonPropertyName("image")]
    public ImageData? Image { get; init; }
}
