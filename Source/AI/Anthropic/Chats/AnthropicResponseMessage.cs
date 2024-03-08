namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicResponseMessage {
    [JsonPropertyName("content")]
    public required AnthropicMessageContent[] Content { get; set; } = [];

    [JsonPropertyName("finish_reason")]
    public string StopReason { get; set; } = string.Empty;
}
