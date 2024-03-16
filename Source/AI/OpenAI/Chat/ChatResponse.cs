namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatResponse(string id) : IChatResponse {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;
    [JsonPropertyName("choices")]
    public ChatResponseChoice[] Choices { get; set; } = [];
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; set; } = string.Empty;
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
}
