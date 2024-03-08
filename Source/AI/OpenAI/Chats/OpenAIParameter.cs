namespace DotNetToolbox.AI.OpenAI.Chats;

public record OpenAIParameter {
    public required string Type { get; init; }
    public string? Description { get; init; }
    public string[]? Enum { get; init; }
}
