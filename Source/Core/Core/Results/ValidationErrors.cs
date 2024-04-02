namespace DotNetToolbox.Results;

public class ValidationErrors : IList<ValidationError>, IList, IReadOnlyList<ValidationError> {
    private readonly List<ValidationError> _errors;
    public ValidationErrors() {
        _errors = [];
    }
    public ValidationErrors(IEnumerable<ValidationError> errors) {
        _errors = [..errors.Distinct()];
    }

    public static implicit operator ValidationErrors(string error) => (ValidationError)error;
    public static implicit operator ValidationErrors(ValidationError error) => [error];
    public static implicit operator ValidationErrors(ValidationError[] errors) => new(errors);
    public static implicit operator ValidationErrors(List<ValidationError> errors) => new(errors);
    public static implicit operator ValidationErrors(HashSet<ValidationError> errors) => new(errors);
    public static implicit operator ValidationError[](ValidationErrors errors) => [..errors];
    public static implicit operator List<ValidationError>(ValidationErrors errors) => [..errors];
    public static implicit operator HashSet<ValidationError>(ValidationErrors errors) => [..errors];

    public static ValidationErrors operator +(ValidationErrors left, ValidationErrors right)
        => new(left.Union(right));
    public static ValidationErrors operator +(ValidationErrors left, ValidationError right)
        => new(left.Union([right]));

    public int Count => _errors.Count;
    public int IndexOf(ValidationError item) => _errors.IndexOf(item);
    public int IndexOf(string value) => IndexOf((ValidationError)value);
    public bool Contains(ValidationError item) => _errors.Contains(item);
    public bool Contains(string value) => Contains((ValidationError)value);

    public void Add(ValidationError item) {
        if (Contains(item)) return;
        _errors.Add(item);
    }
    public void Add(string value) => Add((ValidationError)value);

    public void Insert(int index, ValidationError item) {
        var existingIndex = IndexOf(item);
        if (existingIndex == index) return;
        if (existingIndex != -1) throw new InvalidOperationException("Cannot insert duplicate error.");
        ((IList)_errors).Insert(index, item);
    }
    public void Insert(int index, string value) => Insert(index, (ValidationError)value);

    public bool Remove(ValidationError item) => _errors.Remove(item);
    public void Remove(string value) => Remove((ValidationError)value);

    public void RemoveAt(int index) => _errors.RemoveAt(index);

    public ValidationError this[int index] {
        get => _errors[index];
        set => _errors[index] = value;
    }

    object? IList.this[int index] {
        get => ((IList)_errors)[index];
        set => ((IList)_errors)[index] = value is string s ? (ValidationError)s : value;
    }
    public void Clear() => _errors.Clear();

    public IEnumerator<ValidationError> GetEnumerator() => _errors.GetEnumerator();
    public void CopyTo(ValidationError[] array, int arrayIndex) => _errors.CopyTo(array, arrayIndex);
    public void CopyTo(Array array, int index) => ((ICollection)_errors).CopyTo(array, index);
    public bool IsReadOnly => false;

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_errors).GetEnumerator();
    bool IList.Contains(object? value) => Contains((ValidationError)IsNotNull(value));
    int IList.IndexOf(object? value) => ((IList)_errors).IndexOf(value);
    int IList.Add(object? value) => ((IList)_errors).Add(value);
    void IList.Insert(int index, object? value) => ((IList)_errors).Insert(index, value);
    void IList.Remove(object? value) => ((IList)_errors).Remove(value);
    bool IList.IsFixedSize => ((IList)_errors).IsFixedSize;
    void ICollection<ValidationError>.Clear() => _errors.Clear();
    bool ICollection.IsSynchronized => ((ICollection)_errors).IsSynchronized;
    object ICollection.SyncRoot => ((ICollection)_errors).SyncRoot;
    void IList<ValidationError>.RemoveAt(int index) => RemoveAt(index);
}
