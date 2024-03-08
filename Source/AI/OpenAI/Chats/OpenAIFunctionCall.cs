namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIFunctionCall {
    public required string Name { get; set; }
    public string? Arguments { get; set; }
}
