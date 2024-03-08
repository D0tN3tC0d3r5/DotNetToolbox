namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestMessageContent {
    public OpenAIChatRequestMessageContent(object value) {
        Text = value as string;
        Image = value as OpenAIChatRequestMessageContentImageData;
        Type = Text is null ? "image_url" : "text";
    }

    [JsonPropertyName("type")]
    public string Type { get; init; }
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    [JsonPropertyName("image_url")]
    public OpenAIChatRequestMessageContentImageData? Image { get; init; }
}
