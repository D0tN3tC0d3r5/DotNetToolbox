namespace DotNetToolbox.AI.Anthropic.Chat;

public class AnthropicMessageContent {
    public AnthropicMessageContent(object value) {
        Text = value as string;
        Image = value as AnthropicImageData;
        Type = Text is null ? "image" : "text";
    }

    [JsonPropertyName("type")]
    public string Type { get; init; }
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    [JsonPropertyName("image")]
    public AnthropicImageData? Image { get; init; }
}
