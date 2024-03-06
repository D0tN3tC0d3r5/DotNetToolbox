namespace DotNetToolbox.AI.Anthropic.DataModels;

public class AnthropicCompletionResponse {
    [JsonPropertyName("completion")]
    public string Completion { get; set; } = string.Empty;

    [JsonPropertyName("stop_reason")]
    public string StopReason { get; set; } = string.Empty;

    // Add other properties as needed
}