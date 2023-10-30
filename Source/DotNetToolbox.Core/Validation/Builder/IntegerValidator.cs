namespace System.Validation.Builder;

public class IntegerValidator : Validator<int?>, IIntegerValidator {
    private readonly ValidationCommandFactory _commandFactory;

    public IntegerValidator(int? subject, string source, ValidatorMode mode = ValidatorMode.And)
        : base(subject, source, mode) {
        Connector = new Connector<int?, IntegerValidator>(this);
        _commandFactory = ValidationCommandFactory.For(typeof(int?), Source);
    }

    public IConnector<IntegerValidator> Connector { get; }

    public IConnector<IntegerValidator> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<IntegerValidator> IsNotNull() {
        Negate();
        return IsNull();
    }

    public IConnector<IntegerValidator> MinimumIs(int minimum) {
        Negate();
        return IsLessThan(minimum);
    }

    public IConnector<IntegerValidator> IsGreaterThan(int minimum) {
        var validator = _commandFactory.Create(nameof(IsGreaterThan), minimum);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<IntegerValidator> IsEqualTo(int value) {
        var validator = _commandFactory.Create(nameof(IsEqualTo), value);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<IntegerValidator> IsLessThan(int maximum) {
        var validator = _commandFactory.Create(nameof(IsLessThan), maximum);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<IntegerValidator> MaximumIs(int maximum) {
        Negate();
        return IsGreaterThan(maximum);
    }
}