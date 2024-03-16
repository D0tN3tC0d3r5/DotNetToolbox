namespace Sophia.Services.WorldServiceModel;

public class InformationData {
    public required string? Value { get; set; }
    public required string ValueTemplate { get; set; }
    public required string DefaultText { get; set; }

    public Information ToModel()
        => new() {
            Value = Value,
            ValueTemplate = ValueTemplate,
            DefaultText = DefaultText,
        };
}
