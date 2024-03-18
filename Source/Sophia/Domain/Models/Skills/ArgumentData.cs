namespace Sophia.Models.Skills;

public class ArgumentData {
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public ArgumentType Type { get; set; }
    public string? Description { get; set; }
    public List<string> Options { get; set; } = [];
    public bool IsRequired { get; set; }

    public Argument ToModel()
        => new() {
            Name = Name,
            Type = Type,
            Description = Description,
            Options = Options.ToArray(),
            IsRequired = IsRequired,
        };
}
