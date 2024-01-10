namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsValidPasswordCommand(IValidatable policy, string source) : ValidationCommand(source) {
    public override Result Validate(object? subject) {
        if (subject is not string password) return Result.Success();
        var context = new Dictionary<string, object?> { ["Password"] = password };
        var policyResult = policy.Validate(context);
        if (policyResult.IsSuccess) return Result.Success();
        var result = Result.InvalidData(Source, MustBeAValidPassword);
        return policyResult.Errors.Aggregate(result, (current, error) => current + error);
    }
}
