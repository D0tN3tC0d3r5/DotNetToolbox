namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatResponseChoice {
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("finish_reason")]
    public string? StopReason { get; set; }
    [JsonPropertyName("message")]
    public ChatResponseMessage Message { get; set; } = default!;
    [JsonPropertyName("delta")]
    public ChatResponseMessage? Delta { set => Message = value ?? Message; }
}
