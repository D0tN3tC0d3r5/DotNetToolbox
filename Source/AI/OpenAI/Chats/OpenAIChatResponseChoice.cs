namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatResponseChoice {
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("finish_reason")]
    public string? StopReason { get; set; }
    [JsonPropertyName("message")]
    public OpenAIChatResponseMessage Message { get; set; } = default!;
    [JsonPropertyName("delta")]
    public OpenAIChatResponseMessage? Delta { set => Message = value ?? Message; }
}
