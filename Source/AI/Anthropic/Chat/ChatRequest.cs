namespace DotNetToolbox.AI.Anthropic.Chat;
public class ChatRequest : IChatRequest {
    [JsonPropertyName("model")]
    public required string Model { get; init; }
    [JsonPropertyName("messages")]
    public required RequestMessage[] Messages { get; init; }
    [JsonPropertyName("max_tokens")]
    public required uint MaximumOutputTokens { get; init; }
    [JsonPropertyName("system")]
    public required string System { get; set; }
    [JsonPropertyName("metadata")]
    public ChatMetadata? Metadata { get; set; }
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
