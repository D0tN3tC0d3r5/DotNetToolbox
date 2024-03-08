namespace DotNetToolbox.AI.OpenAI.Chats;

public record OpenAITool(OpenAIFunction Function) {
    public string Type { get; init; } = "function";
}
