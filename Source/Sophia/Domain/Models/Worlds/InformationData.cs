namespace Sophia.Models.Worlds;

public class InformationData {
    public string? Value { get; set; }
    public string ValueTemplate { get; set; } = "{0}";
    public string DefaultText { get; set; } = string.Empty;

    public Information ToModel()
        => new() {
            Value = Value,
            ValueTemplate = ValueTemplate,
            DefaultText = DefaultText,
        };
}
