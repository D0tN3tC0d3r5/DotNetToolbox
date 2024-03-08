namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIToolCall {
    public required string Id { get; set; }
    public required string Type { get; set; }
    public required OpenAIFunctionCall Function { get; set; }
}
