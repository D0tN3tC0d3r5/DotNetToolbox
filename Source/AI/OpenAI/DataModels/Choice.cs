namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record Choice {
    [JsonPropertyName("index")]
    public int Index { get; init; }
    [JsonPropertyName("message")]
    public Chats.Message? Message { get; init; }
    [JsonPropertyName("delta")]
    public Chats.Message? Delta { get; init; }
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; init; }
}
