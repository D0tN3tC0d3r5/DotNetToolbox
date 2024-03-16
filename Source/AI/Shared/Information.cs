namespace DotNetToolbox.AI.Shared;

public class Information : IValidatableAsync {
    public string? Value { get; set; }

    [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
    public required string ValueTemplate { get; set; }

    public required string DefaultText { get; set; }

    public Task<Result> Validate(IDictionary<string, object?>? context = null, CancellationToken token = default)
        => Result.SuccessTask();

    public override string ToString()
        => string.IsNullOrWhiteSpace(Value)
               ? DefaultText
               : string.Format(ValueTemplate, Value);
}
