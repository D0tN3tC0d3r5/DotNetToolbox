namespace DotNetToolbox.AI.Chats;

public class Information : IValidatableAsync {
    public object? Value { get; set; }

    [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
    public required string ValueTemplate { get; init; }
    
    public required string NullText { get; init; }
    
    public Task<Result> Validate(IDictionary<string, object?>? context = null, CancellationToken token = default)
        => Result.SuccessTask();

    public override string ToString()
        => Value is null || (Value is string s && string.IsNullOrWhiteSpace(s))
               ? NullText
               : string.Format(ValueTemplate, Value);
}
