namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIResponseMessage {
    [JsonPropertyName("content")]
    public required object Content { get; set; } = default!;

    [JsonPropertyName("tool_calls")]
    public OpenAIToolCall[]? ToolCalls { set => Content = (object?)value ?? Content; }

    public object ToContent()
        => Content switch {
            OpenAIToolCall[] => Content,
            string txt => new Message("assistant", [new("text", txt)]),
            _ => throw new NotSupportedException(),
        };
}
