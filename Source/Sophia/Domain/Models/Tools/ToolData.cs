namespace Sophia.Models.Tools;

public class ToolData
    : IEntity<int> {
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9_-]+$", ErrorMessage = "Name can only start with a letter or a number, and can only contain letters, numbers, underscores, and hyphens.")]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }
    public List<ArgumentData> Arguments { get; set; } = [];

    public Tool ToModel()
        => new() {
            Id = Id,
            Name = Name,
            Description = Description,
            Arguments = Arguments.AsIndexed().ToList(x => x.Value.ToModel(x.Index)),

        };

    public string? ValidateSignature(IReadOnlyList<ToolData> existingTools)
        => existingTools.Any(t => t.Matches(this))
               ? "A tool with the same signature already exists."
               : null;

    private bool Matches(ToolData other)
        => Name == other.Name
        && Id != other.Id
        && Arguments.Select(a => a.Type).SequenceEqual(other.Arguments.Select(a => a.Type));
}
