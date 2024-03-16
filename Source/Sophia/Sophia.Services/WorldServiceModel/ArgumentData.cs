namespace Sophia.Services.WorldServiceModel;

public class ArgumentData {
    public string Name { get; set; } = string.Empty;
    public ArgumentType Type { get; set; }
    public string? Description { get; set; }
    public string[]? Options { get; set; }
    public bool IsRequired { get; set; }

    public Argument ToModel()
        => new() {
            Name = Name,
            Type = Type,
            Description = Description,
            Options = Options,
            IsRequired = IsRequired,
        };
}
