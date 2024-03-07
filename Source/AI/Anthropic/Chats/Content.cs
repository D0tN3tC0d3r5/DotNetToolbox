namespace DotNetToolbox.AI.Anthropic.Chats;

public class Content {
    public Content(string text) {
        Type = "text";
        Text = text;
    }

    public Content(Image image) {
        Type = "image";
        Image = image;
    }

    [JsonPropertyName("type")]
    public string Type { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("image")]
    public Image? Image { get; init; }
}
