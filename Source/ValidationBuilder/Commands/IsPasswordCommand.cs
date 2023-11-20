namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class IsPasswordCommand
    : ValidationCommand {
    private readonly IPasswordPolicy _policy;

    public IsPasswordCommand(IPasswordPolicy policy, string source)
        : base(source) {
        _policy = policy;
    }

    public override Result Validate(object? subject) {
        if (subject is not string password) return Result.Success();
        var policyResult = _policy.Enforce(password);
        if (policyResult.IsSuccess) return Result.Success();
        var result = Result.Invalid(Source, MustBeAValidPassword, GetErrorMessageArguments(subject));
        return policyResult.Errors.Aggregate(result, (current, error) => current + error);
    }
}