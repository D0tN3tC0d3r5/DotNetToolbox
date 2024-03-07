namespace DotNetToolbox.AI.OpenAI.Chats;

public class Content {
    public Content(string text) {
        Type = "text";
        Text = text;
    }

    public Content(Image image) {
        Type = "image_url";
        Image = image;
    }

    [JsonPropertyName("type")]
    public string Type { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("image_url")]
    public Image? Image { get; init; }
}
