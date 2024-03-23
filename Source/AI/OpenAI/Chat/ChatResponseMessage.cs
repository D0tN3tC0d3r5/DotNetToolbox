using System.Diagnostics.CodeAnalysis;

namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatResponseMessage {
    private object? _content = default!;

    [JsonPropertyName("content")]
    public required object Content {
        get => _content ?? ToolCalls ?? null!;
        [MemberNotNull(nameof(_content))]
        set => _content = value;
    }

    [JsonPropertyName("tool_calls")]
    public ChatResponseToolRequest[]? ToolCalls { get; set; }

    public object ToContent()
        => Content switch {
            ChatResponseToolRequest[] => Content,
            Message => Content,
            string txt => new Message("assistant", txt),
            _ => throw new NotSupportedException(),
        };
}
