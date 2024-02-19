namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record CompletionRequest(string Model = "gpt-4-turbo-preview") {
    public Message[] Messages { get; set; } = [];
    public decimal? FrequencyPenalty { get; set; }
    public decimal? PresencePenalty { get; set; }
    public decimal? Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public int? MaximumTokensPerMessage { get; set; }
    [JsonPropertyName("n")]
    public int? NumberOfChoices { get; set; }
    [JsonPropertyName("stop")]
    public string[]? StopSignals { get; set; }
    [JsonPropertyName("top_p")]
    public decimal? TopProbability { get; set; }
    [JsonPropertyName("stream")]
    public bool? UseStreaming { get; set; }
    public Tool[]? Tools { get; set; }
    public JsonElement? ToolChoice { get; set; }
    public JsonElement? ResponseFormat { get; set; }
}
