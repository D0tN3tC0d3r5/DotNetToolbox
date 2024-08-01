namespace DotNetToolbox.AI.Anthropic.Chats;

public class ChatResponseMessage {
    [JsonPropertyName("content")]
    public required MessageContent[] Content { get; set; } = [];

    [JsonPropertyName("finish_reason")]
    public string StopReason { get; set; } = string.Empty;
}
