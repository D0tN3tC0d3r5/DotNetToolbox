namespace DotNetToolbox.AI.Models;

public interface IModel {
    string Id { get; }
    string Name { get; }
    uint MaximumContextSize { get; }
    uint MaximumOutputTokens { get; }
    DateOnly CreatedOn { get; }
    string? OwnedBy { get; }
}
