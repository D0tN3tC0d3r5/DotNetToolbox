namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsPasswordCommand(IPasswordPolicy policy, string source) : ValidationCommand(source) {
    public override Result Validate(object? subject) {
        if (subject is not string password) return Result.Success();
        var policyResult = policy.Enforce(password);
        if (policyResult.IsSuccess) return Result.Success();
        var result = Result.Invalid(Source, MustBeAValidPassword);
        return policyResult.Errors.Aggregate(result, (current, error) => current + error);
    }
}