namespace DotNetToolbox.AI.OpenAI.Model;

public record Model {
    public required string Id { get; init; }
    public long Created { get; init; }
    public string? OwnedBy { get; init; }
}
