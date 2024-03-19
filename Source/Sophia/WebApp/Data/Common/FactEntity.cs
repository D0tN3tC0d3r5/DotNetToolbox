namespace Sophia.WebApp.Data.Common;

[Owned]
public class FactEntity {
    [MaxLength(100)]
    public string? Value { get; set; }
    [MaxLength(1000)]
    public string ValueTemplate { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string DefaultText { get; set; } = string.Empty;

    public FactData ToDto()
        => new() {
            DefaultText = DefaultText,
            Value = Value,
            ValueTemplate = ValueTemplate,
        };
}
