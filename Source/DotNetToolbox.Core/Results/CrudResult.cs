namespace System.Results;

public sealed record CrudResult
    : Result<CrudResult, CrudResultType>
    , ICrudResult
    , ICreateCrudResults<CrudResult>
    , IResultOperators<CrudResult>
{
    private CrudResult(CrudResultType type, IEnumerable<IValidationError>? errors = null)
        : base(CrudResultType.ValidationFailure, type, errors)
    {
    }

    public bool IsFailure => HasErrors;
    public bool IsSuccess => !HasErrors && Type is CrudResultType.Success;
    public bool IsNotFound => !HasErrors && Type is CrudResultType.NotFound;
    public bool IsConflict => !HasErrors && Type is CrudResultType.Conflict;

    public static CrudResult Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static CrudResult Invalid(IEnumerable<IValidationError> errors)
        => errors.ToArray();
    public static CrudResult Invalid(IValidationError error)
        => (ValidationError)error;
    public static CrudResult Invalid(IValidationResult result)
        => (ValidationResult)result;

    public static CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new (CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);

    public static implicit operator CrudResult(List<IValidationError> errors) => errors.ToArray();
    public static implicit operator CrudResult(ValidationError error) => new [] { error };
    public static implicit operator CrudResult(IValidationError[] errors) => (ValidationResult)errors;
    public static implicit operator CrudResult(ValidationResult result)
        => new(CrudResultType.ValidationFailure, IsNotNullOrEmpty(result.ValidationErrors));

    public static CrudResult operator +(CrudResult left, IValidationResult right)
        => new(left.Type, left.ValidationErrors.Merge(right.ValidationErrors));
    public static CrudResult operator +(CrudResult left, IEnumerable<IValidationError> errors)
        => left.ValidationErrors.Merge(errors).ToArray();
    public static CrudResult operator +(CrudResult left, IValidationError error)
        => new(left.Type, left.ValidationErrors.Merge(error));

    public override bool Equals(CrudResult? other)
        => base.Equals(other);

    public override int GetHashCode()
        => base.GetHashCode();
}

public record CrudResult<TValue>
    : Result<CrudResult<TValue>, CrudResultType>
    , ICrudResult<TValue>
    , ICreateValuedCrudResults<CrudResult<TValue>, TValue>
    , IResultOperators<CrudResult<TValue>>
{
    private CrudResult(CrudResultType type, TValue? value = default, IEnumerable<IValidationError>? errors = null)
        : base(CrudResultType.ValidationFailure, type, errors)
    {
        Value = value;
    }

    public bool IsFailure => HasErrors;
    public bool IsSuccess => !HasErrors && Type is CrudResultType.Success;
    public bool IsNotFound => !HasErrors && Type is CrudResultType.NotFound;
    public bool IsConflict => !HasErrors && Type is CrudResultType.Conflict;
    public TValue? Value { get; }

    public static implicit operator CrudResult<TValue>(TValue value)
        => new(CrudResultType.Success, IsNotNull(value));

    public static implicit operator CrudResult<TValue>(List<IValidationError> errors) => errors.ToArray();
    public static implicit operator CrudResult<TValue>(ValidationError error) => new[] { error };
    public static implicit operator CrudResult<TValue>(IValidationError[] errors) => (ValidationResult)errors;
    public static implicit operator CrudResult<TValue>(ValidationResult result)
        => new(CrudResultType.ValidationFailure, default, IsNotNullOrEmpty(result.ValidationErrors));
    public static implicit operator ValidationResult(CrudResult<TValue> result)
        => result.ValidationErrors.ToArray();

    public static CrudResult<TValue> operator +(CrudResult<TValue> left, IValidationResult right)
        => new(left.Type, left.Value, left.ValidationErrors.Merge(right.ValidationErrors));
    public static CrudResult<TValue> operator +(CrudResult<TValue> left, IEnumerable<IValidationError> errors)
        => new(left.Type, left.Value, left.ValidationErrors.Merge(errors));
    public static CrudResult<TValue> operator +(CrudResult<TValue> left, IValidationError error)
        => new(left.Type, left.Value, left.ValidationErrors.Merge(error));

    public ICrudResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> map)
        => Value is null
            ? CrudResult<TNewValue>.NotFound()
            : new(Type, map(Value), ValidationErrors);

    public override bool Equals(CrudResult<TValue>? other)
        => base.Equals(other)
           && (Value?.Equals(other!.Value) ?? other!.Value is null);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value?.GetHashCode() ?? 0);

    public static CrudResult<TValue> Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static CrudResult<TValue> Invalid(IValidationResult result)
        => (ValidationResult)result;
    public static CrudResult<TValue> Invalid(IEnumerable<IValidationError> errors)
        => errors.ToArray();
    public static CrudResult<TValue> Invalid(IValidationError error)
        => (ValidationError)error;

    public static CrudResult<TValue> Success(TValue value) => new(CrudResultType.Success, IsNotNull(value));
    public static CrudResult<TValue> NotFound() => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict(TValue value) => new(CrudResultType.Conflict, IsNotNull(value));
}
