namespace DotNetToolbox.AI.OpenAI.Chats;

public record OpenAIFunction {
    public required string Name { get; init; }
    public required string Description { get; init; }
    public OpenAIParameterList? Parameters { get; init; }
}
