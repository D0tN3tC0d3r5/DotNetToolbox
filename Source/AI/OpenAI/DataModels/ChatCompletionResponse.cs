namespace DotNetToolbox.AI.OpenAI.DataModels;

internal class ChatCompletionResponse {
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    [JsonPropertyName("model")]
    public string? Model { get; init; }
    [JsonPropertyName("choices")]
    public Choice[] Choices { get; init; } = [];
    [JsonPropertyName("created")]
    public int Created { get; init; }
    [JsonPropertyName("system_fingerprint")]
    public string? SystemFingerprint { get; init; }
    [JsonPropertyName("usage")]
    public Usage? Usage { get; init; }
}
