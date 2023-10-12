namespace System.Results;

public sealed record ValidationResult
    : Result<ValidationResult, ValidationResultType>
    , IValidationResult
    , ICreateValidationResults<ValidationResult>
    , IResultOperators<ValidationResult> {
    private ValidationResult(IEnumerable<ValidationError>? errors = null)
        : base(ValidationResultType.Failure, ValidationResultType.Success, errors) { }

    public bool IsFailure => IsInvalid;
    public bool IsSuccess => IsValid;

    public static ValidationResult Failure([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new(new ValidationError(message, args));
    public static ValidationResult Failure(IValidationResult result)
        => new((ValidationResult)result.Errors.ToArray());
    public static ValidationResult Failure(IEnumerable<ValidationError> errors)
        => errors.ToArray();
    public static ValidationResult Failure(ValidationError error)
        => error;
    public static ValidationResult Success() => new();

    public static implicit operator ValidationResult(Dictionary<string, string[]> errors)
        => errors.SelectMany(i => i.Value.Select(msg => new ValidationError(msg, i.Key))).ToArray();
    public static implicit operator ValidationResult(List<ValidationError> errors)
        => errors.ToArray();
    public static implicit operator ValidationResult(ValidationError error)
        => new[] { error };
    public static implicit operator ValidationResult(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator ValidationResult(CrudResult result)
        => result.Errors.ToArray();
    public static implicit operator ValidationResult(HttpResult result)
        => result.Errors.ToArray();
    public static implicit operator ValidationResult(SignInResult result)
        => result.Errors.ToArray();

    public static ValidationResult operator +(ValidationResult left, IValidationResult right)
        => left.Errors.Merge(right.Errors).ToArray();
    public static ValidationResult operator +(ValidationResult left, IEnumerable<ValidationError> errors)
        => left.Errors.Merge(errors).ToArray();
    public static ValidationResult operator +(ValidationResult left, ValidationError error)
        => left.Errors.Merge(error).ToArray();

    public override bool Equals(ValidationResult? other)
        => base.Equals(other);

    public override int GetHashCode()
        => base.GetHashCode();
}
