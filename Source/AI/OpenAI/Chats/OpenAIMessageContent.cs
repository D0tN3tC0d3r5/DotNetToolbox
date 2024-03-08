namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIMessageContent {
    public OpenAIMessageContent(object value) {
        Text = value as string;
        Image = value as OpenAIImageData;
        Type = Text is null ? "image_url" : "text";
    }

    [JsonPropertyName("type")]
    public string Type { get; init; }
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    [JsonPropertyName("image_url")]
    public OpenAIImageData? Image { get; init; }
}
