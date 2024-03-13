namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatResponse(string id) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;
    [JsonPropertyName("choices")]
    public OpenAIChatResponseChoice[] Choices { get; set; } = [];
    [JsonPropertyName("created")]
    public int Created { get; set; }
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; set; } = string.Empty;
    [JsonPropertyName("usage")]
    public OpenAIChatResponseUsage? Usage { get; set; }
}
