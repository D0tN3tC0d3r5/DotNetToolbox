namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record CompletionRequest : ChatOptions {
    public required Prompt[] Messages { get; init; }

    [JsonPropertyName("max_tokens")]
    public override int? MaximumNumberOfTokensPerMessage { get; init; }
    [JsonPropertyName("n")]
    public override int? NumberOfChoices { get; init; }
    [JsonPropertyName("stop")]
    public override string[]? StopSignals { get; init; }
    [JsonPropertyName("top_p")]
    public override int? TopProbability { get; init; }

    [JsonPropertyName("stream")]
    public bool StreamResponse { get; init; }
    [JsonPropertyName("tool_choice")]
    public JsonElement? ResponseType { get; init; }
    public JsonElement? ResponseFormat { get; init; }
}
