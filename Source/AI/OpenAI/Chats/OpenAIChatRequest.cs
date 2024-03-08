﻿namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequest {
    [JsonPropertyName("model")]
    public required string Model { get; init; }
    [JsonPropertyName("messages")]
    public OpenAIRequestMessage[] Messages { get; init; } = [];
    [JsonPropertyName("frequency_penalty")]
    public decimal? FrequencyPenalty { get; set; }
    [JsonPropertyName("presence_penalty")]
    public decimal? PresencePenalty { get; set; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public int? MaximumTokensPerMessage { get; set; }
    [JsonPropertyName("n")]
    public int? NumberOfChoices { get; set; }
    [JsonPropertyName("stop")]
    public string[]? StopSequences { get; set; }
    [JsonPropertyName("top_p")]
    public decimal? MinimumTokenProbability { get; set; }
    [JsonPropertyName("stream")]
    public bool? UseStreaming { get; set; }
    [JsonPropertyName("tools")]
    public OpenAITool[]? Tools { get; set; }
    [JsonPropertyName("tool_choice")]
    public JsonElement? ToolChoice { get; set; }
    [JsonPropertyName("response_format")]
    public JsonElement? ResponseFormat { get; set; }
}