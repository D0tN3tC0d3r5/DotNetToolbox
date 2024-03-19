namespace Sophia.Models.Tools;

public class ArgumentData {
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Name can only contain letters, numbers, underscores, and hyphens.")]
    public string Name { get; set; } = string.Empty;
    public ArgumentType Type { get; set; }
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }
    public List<string> Choices { get; set; } = [];
    public bool IsRequired { get; set; }

    public Argument ToModel(uint index)
        => new() {
            Index = index,
            Name = Name,
            Type = Type,
            Description = Description,
            Options = [.. Choices],
            IsRequired = IsRequired,
        };

    public string? ValidateChoices() {
        if (Type != ArgumentType.Enum)
            return null;
        if (Choices.Count == 0)
            return "Choices cannot be empty when the argument type is Enum.";
        if (Choices.Count > 20)
            return "The maximum number of choices allowed is 20.";
        if (Choices.Any(string.IsNullOrWhiteSpace))
            return "Choices cannot contain empty or whitespace strings.";
        if (Choices.Any(choice => choice.Length > 50))
            return "Each choice cannot exceed 50 characters.";
        return Choices.Distinct().Count() != Choices.Count ? "Choices must be unique within the same tool." : null;
    }
}
