namespace DotNetToolbox.AI.OpenAI.Models;

internal record OpenAIModel {
    public required string Id { get; init; }
    public long Created { get; init; }
    public string? OwnedBy { get; init; }
}
