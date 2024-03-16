namespace Sophia.Services.WorldServiceModel;

public class ArgumentData {
    public required string Name { get; set; }
    public required string Type { get; set; }
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
