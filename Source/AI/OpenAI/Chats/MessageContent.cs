namespace DotNetToolbox.AI.OpenAI.Chats;

public class MessageContent() {
    [SetsRequiredMembers]
    public MessageContent(object value) : this() {
        Text = value as string;
        Image = value as ImageData;
        Type = Text is null ? "image_url" : "text";
    }

    [JsonPropertyName("type")]
    public required string Type { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("image_url")]
    public ImageData? Image { get; set; }
}
