namespace DotNetToolbox.AI.Anthropic.Chats;
public class AnthropicChatRequest {
    [JsonPropertyName("model")]
    public required string Model { get; init; }
    [JsonPropertyName("messages")]
    public required AnthropicRequestMessage[] Messages { get; init; }
    [JsonPropertyName("max_tokens")]
    public required int MaximumTokensPerMessage { get; init; }
    [JsonPropertyName("system")]
    public string? System { get; set; }
    [JsonPropertyName("metadata")]
    public AnthropicChatMetadata? Metadata { get; set; }
    [JsonPropertyName("stop_sequences")]
    public string[]? StopSequences { get; set; }
    [JsonPropertyName("stream")]
    public bool? UseStreaming { get; set; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; set; }
    [JsonPropertyName("top_p")]
    public decimal? MinimumTokenProbability { get; set; }

    [JsonPropertyName("top_k")]
    public decimal? MaximumTokenSamples { get; set; }
}
