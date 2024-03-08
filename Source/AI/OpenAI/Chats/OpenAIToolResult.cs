namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIToolResult(string toolCallId, string value) {
    public string CallId { get; set; } = toolCallId;
    public string? Value { get; set; } = value;
}
