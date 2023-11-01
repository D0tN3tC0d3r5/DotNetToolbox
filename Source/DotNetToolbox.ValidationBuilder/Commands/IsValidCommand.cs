namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsValidCommand
    : ValidationCommand {
    public IsValidCommand(string source)
        : base(source) {
    }

    public override Result Validate(object? subject) {
        var result = Result.Success();
        return subject is not IValidatable v
                   ? result
                   : v.Validate().Errors
                      .Aggregate(result, (current, error) => current + error with { Source = $"{Source}.{error.Source}", });
    }
}