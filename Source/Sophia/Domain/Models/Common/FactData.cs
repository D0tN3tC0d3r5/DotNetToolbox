namespace Sophia.Models.Common;

public class FactData {
    [Required(AllowEmptyStrings = true)]
    [MaxLength(250)]
    public string DefaultText { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string ValueTemplate { get; set; } = "{0}";
    public string? Value { get; set; }

    public Fact ToModel()
        => new() {
            Value = Value,
            ValueTemplate = ValueTemplate,
            DefaultText = DefaultText,
        };
}
