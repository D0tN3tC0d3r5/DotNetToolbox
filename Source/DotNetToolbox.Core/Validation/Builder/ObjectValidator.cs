namespace System.Validation.Builder;

public class ObjectValidator : Validator<object?>, IObjectValidator {
    private readonly ValidationCommandFactory _commandFactory;

    public ObjectValidator(object? subject, string source, ValidatorMode mode = ValidatorMode.And)
        : base(subject, source, mode) {
        Connector = new(this);
        _commandFactory = ValidationCommandFactory.For(typeof(int?), Source);
    }

    public Connector<object?, ObjectValidator> Connector { get; }

    public IConnector<ObjectValidator> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<ObjectValidator> IsNotNull() {
        Negate();
        return IsNull();
    }
}
