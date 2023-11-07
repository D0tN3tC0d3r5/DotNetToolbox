using System.ComponentModel;

namespace System.Results;

public class CrudResult : ResultBase {
    protected CrudResult(CrudResultType? type, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Type = HasErrors ? null : type;
    }
    protected CrudResultType? Type { get; }

    public static CrudResult Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static CrudResult Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static CrudResult Invalid(Result result)
        => new(default, result.Errors);

    public bool IsInvalid => HasErrors;
    public bool IsSuccess => !HasErrors && Type == CrudResultType.Success;
    public bool WasNotFound => !HasErrors && Type == CrudResultType.NotFound;
    public bool HasConflict => !HasErrors && Type == CrudResultType.Conflict;

    public static CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new(CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);

    public static implicit operator CrudResult(List<ValidationError> errors)
        => new(default, errors);
    public static implicit operator CrudResult(ValidationError[] errors)
        => new(default, errors);
    public static implicit operator CrudResult(ValidationError error)
        => new(default, [ error, ]);

    public static CrudResult operator +(CrudResult left, Result right)
        => new(left.Type, left.Errors.Union(right.Errors));

    public static CrudResult<TValue> Success<TValue>(TValue value)
        => new(CrudResultType.Success, IsNotNull(value));
    public static CrudResult<TValue> NotFound<TValue>()
        => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict<TValue>(TValue value)
        => new(CrudResultType.Conflict, IsNotNull(value));
    public static CrudResult<TValue> Invalid<TValue>(TValue value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(value, string.Empty, message, args);
    public static CrudResult<TValue> Invalid<TValue>(TValue value, string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(value, new ValidationError(source, message, args));
    public static CrudResult<TValue> Invalid<TValue>(TValue value, Result result)
        => new(default, IsNotNull(value), result.Errors);

    public static bool operator ==(CrudResult left, CrudResultType type)
        => left.Type == type;
    public static bool operator !=(CrudResult left, CrudResultType type)
        => left.Type != type;
    public static bool operator ==(CrudResult left, CrudResult? right)
        => left.Equals(right);
    public static bool operator !=(CrudResult left, CrudResult? right)
        => !left.Equals(right);
    public override bool Equals([NotNullWhen(true)] object? obj)
        => base.Equals(obj)
        || obj is CrudResult r && Equals(r);
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Type);

    private bool Equals(CrudResult? other)
        => base.Equals(other)
        && Type == other.Type;
}

public class CrudResult<TValue> : CrudResult {
    internal CrudResult(CrudResultType? type, TValue? value = default, IEnumerable<ValidationError>? errors = null)
        : base(type, errors) {
        Value = Type == CrudResultType.NotFound ? default :  value;
    }

    public TValue? Value { get; init; }

    public static implicit operator CrudResult<TValue>(TValue value) => new(CrudResultType.Success, value);
    public static implicit operator CrudResult<TValue>(Result<TValue> result) => new(CrudResultType.Success, result.Value, result.Errors);

    public static CrudResult<TValue> operator +(CrudResult<TValue> left, Result right)
        => new(left.Type, left.Value, left.Errors.Union(right.Errors));

    public static bool operator ==(CrudResult<TValue> left, CrudResultType type)
        => left.Type == type;
    public static bool operator !=(CrudResult<TValue> left, CrudResultType type)
        => left.Type != type;
    public static bool operator ==(CrudResult<TValue> left, CrudResult<TValue>? right)
        => left.Equals(right);
    public static bool operator !=(CrudResult<TValue> left, CrudResult<TValue>? right)
        => !left.Equals(right);
    public static bool operator ==(CrudResult<TValue> left, TValue value)
        => left.IsSuccess && (left.Value?.Equals(value) ?? false);
    public static bool operator !=(CrudResult<TValue> left, TValue value)
        => !left.IsSuccess || !(left.Value?.Equals(value) ?? false);
    public override bool Equals([NotNullWhen(true)] object? obj)
        => base.Equals(obj)
        || obj is CrudResult<TValue> r && Equals(r);
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

    private bool Equals(CrudResult<TValue>? other)
        => base.Equals(other)
        && (Value?.Equals(other.Value) ?? other.Value is null);

    public CrudResult<TOutput> MapTo<TOutput>(Func<TValue, TOutput> map)
        => new(Type, Value is null ? default : map(Value), Errors);
}
