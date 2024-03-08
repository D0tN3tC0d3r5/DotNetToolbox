namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatResponse {
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    [JsonPropertyName("model")]
    public string? Model { get; init; }
    [JsonPropertyName("choices")]
    public OpenAIChatResponseChoice[] Choices { get; init; } = [];
    [JsonPropertyName("created")]
    public int Created { get; init; }
    [JsonPropertyName("system_fingerprint")]
    public string? SystemFingerprint { get; init; }
    [JsonPropertyName("usage")]
    public OpenAIChatResponseUsage? Usage { get; init; }
}
