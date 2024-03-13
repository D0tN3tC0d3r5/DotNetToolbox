namespace DotNetToolbox.AI.Anthropic.Chat;

public class AnthropicChatResponse {
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    [JsonPropertyName("model")]
    public string? Model { get; init; }
    [JsonPropertyName("completion")]
    public required AnthropicMessageContent[] Completion { get; set; }
    [JsonPropertyName("stop_reason")]
    public required string StopReason { get; init; }
    [JsonPropertyName("stop_sequence")]
    public required string StopSequence { get; init; }
    [JsonPropertyName("usage")]
    public required AnthropicChatUsage Usage { get; init; }
}
