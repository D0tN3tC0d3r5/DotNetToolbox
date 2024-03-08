namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestMessageToolCallResult(string toolCallId, string value) {
    public string CallId { get; set; } = toolCallId;
    public string? Value { get; set; } = value;
}
