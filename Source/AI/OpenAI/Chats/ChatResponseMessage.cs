﻿namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatResponseMessage {
    private object? _content;

    [JsonPropertyName("content")]
    public required object Content {
        get => _content ?? ToolCalls ?? null!;
        [MemberNotNull(nameof(_content))]
        set => _content = value;
    }

    [JsonPropertyName("tool_calls")]
    public ChatResponseToolRequest[]? ToolCalls { get; set; }

    public object ToMessage()
        => Content switch {
            ChatResponseToolRequest[] => Content,
            Message => Content,
            string txt => new Message(MessageRole.Assistant, txt),
            _ => throw new NotSupportedException(),
        };
}
