namespace DotNetToolbox.AI.OpenAI.Chat;

public class MessageContent {
    public MessageContent(object value) {
        Text = value as string;
        Image = value as ImageData;
        Type = Text is null ? "image_url" : "text";
    }

    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("image_url")]
    public ImageData? Image { get; set; }
}
