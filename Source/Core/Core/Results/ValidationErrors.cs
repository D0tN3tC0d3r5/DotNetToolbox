namespace DotNetToolbox.Results;

public class ValidationErrors : List<ValidationError> {
    public ValidationErrors() {
    }
    public ValidationErrors(IEnumerable<ValidationError> errors) {
        AddRange(errors);
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

    public new void Add(ValidationError value) {
        if (!Contains(value))
            base.Add(value);
    }

    public void Add(string value) => Add((ValidationError)value);

    public new void Insert(int index, ValidationError value) {
        if (!Contains(value)) {
            base.Insert(index, value);
            return;
        }

        var oldIndex = base.IndexOf(value);
        if (index != oldIndex) RemoveAt(oldIndex);
        base.Insert(index, value);
    }

    public new void AddRange(IEnumerable<ValidationError> values) {
        foreach (var value in values.Distinct())
            Add(value);
    }

    public void Insert(int index, string value) => Insert(index, (ValidationError)value);
    public void Remove(string value) => Remove((ValidationError)value);

    public void CopyTo(Array array, int index) => ((ICollection)this).CopyTo(array, index);
}
