namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record CompletionRequest(string Model = "gpt-3.5-turbo-1106") {
    public Prompt[] Messages { get; set; } = [];
    public decimal FrequencyPenalty { get; set; }
    public decimal PresencePenalty { get; set; }
    public decimal Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public int MaximumTokensPerMessage { get; set; }
    [JsonPropertyName("n")]
    public int NumberOfChoices { get; set; }
    [JsonPropertyName("stop")]
    public string[] StopSignals { get; set; } = [];
    [JsonPropertyName("top_p")]
    public decimal TopProbability { get; set; }
    [JsonPropertyName("stream")]
    public bool UseStreaming { get; set; }
    public Tool[] Tools { get; set; } = [];
    [JsonPropertyName("tool_choice")]
    public JsonElement? ResponseType { get; set; } = JsonSerializer.SerializeToElement("auto");
    public JsonElement? ResponseFormat { get; set; } = JsonSerializer.SerializeToElement(new { Type = "json_object" });
}
