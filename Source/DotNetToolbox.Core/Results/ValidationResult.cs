namespace System.Results;

public sealed record ValidationResult
    : Result<ValidationResult, ValidationResultType>
    , IValidationResult
    , ICreateValidationResults<ValidationResult>
    , IResultOperators<ValidationResult> {
    private ValidationResult(IEnumerable<IValidationError>? errors = null)
        : base(ValidationResultType.Failure, ValidationResultType.Success, errors) { }

    public bool IsFailure => HasErrors;
    public bool IsSuccess => !HasErrors;

    public static ValidationResult Failure([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new(new ValidationError(message, args));
    public static ValidationResult Failure(IValidationResult result)
        => new((ValidationResult)result.ValidationErrors.ToArray());
    public static ValidationResult Failure(IEnumerable<IValidationError> errors)
        => errors.ToArray();
    public static ValidationResult Failure(IValidationError error)
        => (ValidationError)error;
    public static ValidationResult Success() => new();

    public static implicit operator ValidationResult(Dictionary<string, string[]> errors)
        => errors.SelectMany(i => i.Value.Select(msg => new ValidationError(msg, i.Key))).ToArray();
    public static implicit operator ValidationResult(List<IValidationError> errors)
        => errors.ToArray();
    public static implicit operator ValidationResult(ValidationError error)
        => new[] { error };
    public static implicit operator ValidationResult(IValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator ValidationResult(CrudResult result)
        => result.ValidationErrors.ToArray();
    public static implicit operator ValidationResult(HttpResult result)
        => result.ValidationErrors.ToArray();
    public static implicit operator ValidationResult(SignInResult result)
        => result.ValidationErrors.ToArray();

    public static ValidationResult operator +(ValidationResult left, IValidationResult right)
        => left.ValidationErrors.Merge(right.ValidationErrors).ToArray();
    public static ValidationResult operator +(ValidationResult left, IEnumerable<IValidationError> errors)
        => left.ValidationErrors.Merge(errors).ToArray();
    public static ValidationResult operator +(ValidationResult left, IValidationError error)
        => left.ValidationErrors.Merge(error).ToArray();

    public override bool Equals(ValidationResult? other)
        => base.Equals(other);

    public override int GetHashCode()
        => base.GetHashCode();
}
