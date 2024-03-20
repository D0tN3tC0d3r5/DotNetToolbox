namespace Sophia.Models.Common;

public class FactData {
    public int Id { get; set; }
    [MaxLength(250)]
    public string? DefaultText { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string ValueTemplate { get; set; } = "{0}";
    public string? Value { get; set; }

    public Fact ToModel()
        => new() {
            Value = Value,
            ValueTemplate = ValueTemplate,
            DefaultText = DefaultText ?? string.Empty,
        };

    public string? Validate() {
        var valuePlaceholderCount = ValueTemplate.Split("{0}").Length - 1;
        return valuePlaceholderCount != 1 ? "The value template must contain exactly one value placeholder (\"{0}\")." : null;
    }

    public override string ToString()
        => Value is null
               ? DefaultText?? string.Empty
               : string.Format(ValueTemplate, Value);
}
