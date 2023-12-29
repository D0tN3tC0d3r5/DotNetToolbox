namespace DotNetToolbox.OpenAI.Models;

public record Model {
    public required string Id { get; init; }
    public required string Name { get; init; }
    public bool IsFineTuned { get; init; }
    public ModelType Type { get; init; }
    public DateTimeOffset CreatedOn { get; init; }
    public string? OwnedBy { get; init; }
}
