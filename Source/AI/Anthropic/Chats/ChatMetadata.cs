namespace DotNetToolbox.AI.Anthropic.Chats;

public class ChatMetadata {
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}
