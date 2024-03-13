using System.Diagnostics.CodeAnalysis;

namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatResponseMessage {
    private object? _content = default!;

    [JsonPropertyName("content")]
    public required object Content {
        get => _content ?? ToolCalls ?? null!;
        [MemberNotNull(nameof(_content))]
        set => _content = value;
    }

    [JsonPropertyName("tool_calls")]
    public OpenAIChatResponseToolRequest[]? ToolCalls { get; set; }

    public object ToContent()
        => Content switch {
            OpenAIChatResponseToolRequest[] => Content,
            Message => Content,
            string txt => new Message("assistant", [new MessagePart("text", txt)]),
            _ => throw new NotSupportedException(),
        };
}
