namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record CompletionRequest(string Model = "gpt-3.5-turbo-1106") : ChatOptions(Model) {
    public required Prompt[] Messages { get; init; }

    [JsonPropertyName("max_tokens")]
    public override int? MaximumNumberOfTokensPerMessage { get; init; }
    [JsonPropertyName("n")]
    public override int? NumberOfChoices { get; init; }
    [JsonPropertyName("stop")]
    public override string[] StopSignals { get; init; } = [];
    [JsonPropertyName("top_p")]
    public override int? TopProbability { get; init; }
    [JsonPropertyName("stream")]
    public override bool UseStream { get; init; }
    [JsonPropertyName("tool_choice")]
    public JsonElement? ResponseType { get; init; } = JsonSerializer.SerializeToElement("auto");
    public JsonElement? ResponseFormat { get; init; } = JsonSerializer.SerializeToElement(new { Type = "json_object" });
}
