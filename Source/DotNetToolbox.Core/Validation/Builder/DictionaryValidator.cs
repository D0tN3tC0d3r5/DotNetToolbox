namespace System.Validation.Builder;

public class DictionaryValidator<TKey, TValue>
    : Validator<IDictionary<TKey, TValue?>>
        , IDictionaryValidator<TKey, TValue>
    where TKey : notnull {
    private readonly ValidationCommandFactory _commandFactory;

    public DictionaryValidator(IDictionary<TKey, TValue?>? subject, string source, ValidatorMode mode = ValidatorMode.And)
        : base(subject, source, mode) {
        Connector = new Connector<IDictionary<TKey, TValue?>, DictionaryValidator<TKey, TValue>>(this);
        _commandFactory = ValidationCommandFactory.For(typeof(IDictionary<TKey, TValue?>), Source);
    }

    public IConnector<DictionaryValidator<TKey, TValue>> Connector { get; }

    public IConnector<DictionaryValidator<TKey, TValue>> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DictionaryValidator<TKey, TValue>> IsNotNull() {
        Negate();
        return IsNull();
    }

    public IConnector<DictionaryValidator<TKey, TValue>> IsEmpty() {
        var validator = _commandFactory.Create(nameof(IsEmpty));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DictionaryValidator<TKey, TValue>> IsNotEmpty() {
        Negate();
        return IsEmpty();
    }

    public IConnector<DictionaryValidator<TKey, TValue>> HasAtLeast(int size) {
        var validator = _commandFactory.Create(nameof(HasAtLeast), size);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DictionaryValidator<TKey, TValue>> Has(int size) {
        var validator = _commandFactory.Create(nameof(Has), size);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DictionaryValidator<TKey, TValue>> ContainsKey(TKey key) {
        var validator = _commandFactory.Create(nameof(ContainsKey), key);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DictionaryValidator<TKey, TValue>> HasAtMost(int size) {
        var validator = _commandFactory.Create(nameof(HasAtMost), size);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DictionaryValidator<TKey, TValue>> Each(Func<TValue?, ITerminator> validateUsing) {
        if (Subject is null) return Connector;
        foreach (var key in Subject.Keys)
            AddItemErrors(validateUsing(Subject[key]).Result.Errors, $"{Source}[{key}]");
        return Connector;
    }

    private void AddItemErrors(IEnumerable<ValidationError> errors, string source) {
        foreach (var error in errors) {
            var path = error.Source.Split('.');
            error.Source = $"{source}.{string.Join('.', path[1..])}";
            AddError(error);
        }
    }
}