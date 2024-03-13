namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatFunctionCallResult(string toolCallId, string value) {
    public string CallId { get; set; } = toolCallId;
    public string? Value { get; set; } = value;
}
