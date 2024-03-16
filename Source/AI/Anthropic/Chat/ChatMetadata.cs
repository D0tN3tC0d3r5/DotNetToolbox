namespace DotNetToolbox.AI.Anthropic.Chat;

public class ChatMetadata {
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}
