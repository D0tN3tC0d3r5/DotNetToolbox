namespace DotNetToolbox.AI.Anthropic.DataModels;
public class AnthropicCompletionRequest {
    [JsonPropertyName("model")]
    public string Model { get; set; } = "claude-v1";

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("max_tokens_to_sample")]
    public int? MaxTokensToSample { get; set; }

    [JsonPropertyName("stop_sequences")]
    public List<string>? StopSequences { get; set; }

    // Add other properties as needed
}
