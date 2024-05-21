namespace DotNetToolbox.AI.Anthropic.Chat;

public class ImageData {
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }
}
