namespace DotNetToolbox.AI.Anthropic.Chats;

public record AnthropicChatUsage {
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; init; }
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; init; }
}
