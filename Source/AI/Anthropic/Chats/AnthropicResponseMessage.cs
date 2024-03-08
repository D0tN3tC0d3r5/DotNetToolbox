namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicResponseMessage {
    [JsonPropertyName("content")]
    public required object Content { get; set; } = default!;

    [JsonPropertyName("finish_reason")]
    public string StopReason { get; set; } = string.Empty;

    public object ToContent()
        => Content switch {
            OpenAIToolCall[] => Content,
            AnthropicMessageContent[] parts => new Message("assistant", parts.ToArray(p => new Content(p.Type, ((object?)p.Text ?? p.Image)!))),
            _ => throw new NotSupportedException(),
        };
}
