namespace DotNetToolbox.AI.OpenAI.Model;

internal record OpenAIModel {
    public required string Id { get; init; }
    public long Created { get; init; }
    public string? OwnedBy { get; init; }
}
