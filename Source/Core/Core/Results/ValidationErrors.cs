namespace DotNetToolbox.Results;

public class ValidationErrors : List<ValidationError> {
    private readonly List<ValidationError> _errors;
    public ValidationErrors() {
        _errors = [];
    }
    public ValidationErrors(IEnumerable<ValidationError> errors) {
        _errors = [.. errors.Distinct()];
    }

    public static implicit operator ValidationErrors(string error) => (ValidationError)error;
    public static implicit operator ValidationErrors(ValidationError error) => [error];
    public static implicit operator ValidationErrors(ValidationError[] errors) => new(errors);
    public static implicit operator ValidationErrors(HashSet<ValidationError> errors) => new(errors);
    public static implicit operator ValidationError[](ValidationErrors errors) => [.. errors];
    public static implicit operator HashSet<ValidationError>(ValidationErrors errors) => [.. errors];

    public static ValidationErrors operator +(ValidationErrors left, ValidationErrors right)
        => new(left.Union(right));
    public static ValidationErrors operator +(ValidationErrors left, ValidationError right)
        => new(left.Union([right]));

    public int IndexOf(string value) => IndexOf((ValidationError)value);
    public bool Contains(string value) => Contains((ValidationError)value);

    public void Add(string value) => Add((ValidationError)value);

    public void Insert(int index, string value) => Insert(index, (ValidationError)value);
    public void Remove(string value) => Remove((ValidationError)value);

    public void CopyTo(Array array, int index) => ((ICollection)_errors).CopyTo(array, index);
}
