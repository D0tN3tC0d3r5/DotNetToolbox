namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatResponseMessage {
    [JsonPropertyName("content")]
    public required object Content { get; set; } = default!;

    [JsonPropertyName("tool_calls")]
    public OpenAIChatResponseToolCall[]? ToolCalls { set => Content = (object?)value ?? Content; }

    public object ToContent()
        => Content switch {
            OpenAIChatResponseToolCall[] => Content,
            string txt => new Message("assistant", [new("text", txt)]),
            _ => throw new NotSupportedException(),
        };
}
