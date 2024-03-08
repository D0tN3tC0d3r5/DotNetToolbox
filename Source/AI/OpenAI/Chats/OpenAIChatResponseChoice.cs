namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatResponseChoice {
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("finish_reason")]
    public string? StopReason { get; set; }
    [JsonPropertyName("message")]
    public OpenAIResponseMessage Message { get; set; } = default!;
    [JsonPropertyName("delta")]
    public OpenAIResponseMessage? Delta { set => Message = value ?? Message; }
}
