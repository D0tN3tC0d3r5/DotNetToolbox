namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatRequest {
    [JsonPropertyName("model")]
    public required string Model { get; set; }
    [JsonPropertyName("messages")]
    public OpenAIChatRequestMessage[] Messages { get; set; } = [];
    [JsonPropertyName("frequency_penalty")]
    public decimal? FrequencyPenalty { get; set; }
    [JsonPropertyName("presence_penalty")]
    public decimal? PresencePenalty { get; set; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; set; }
    [JsonPropertyName("n")]
    public int? NumberOfChoices { get; set; }
    [JsonPropertyName("stop")]
    public string[]? StopSequences { get; set; }
    [JsonPropertyName("top_p")]
    public decimal? MinimumTokenProbability { get; set; }
    [JsonPropertyName("stream")]
    public bool? UseStreaming { get; set; }
    [JsonPropertyName("tools")]
    public OpenAIChatRequestTool[]? Tools { get; set; }
    [JsonPropertyName("tool_choice")]
    public OpenAIChatRequestForceToolCall? ForceToolCall { get; set; }
    [JsonPropertyName("response_format")]
    public OpenAIChatRequestResponseFormat? ResponseFormat { get; set; }
}
