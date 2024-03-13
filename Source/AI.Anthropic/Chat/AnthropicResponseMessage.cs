namespace DotNetToolbox.AI.Anthropic.Chat;

public class AnthropicResponseMessage {
    [JsonPropertyName("content")]
    public required AnthropicMessageContent[] Content { get; set; } = [];

    [JsonPropertyName("finish_reason")]
    public string StopReason { get; set; } = string.Empty;
}
