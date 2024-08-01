namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatFunctionCallResult(string toolCallId, string value) {
    public string CallId { get; set; } = toolCallId;
    public string? Value { get; set; } = value;
}
