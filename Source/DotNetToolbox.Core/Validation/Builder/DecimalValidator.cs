namespace System.Validation.Builder;

public class DecimalValidator : Validator<decimal?>, IDecimalValidator {
    private readonly ValidationCommandFactory _commandFactory;

    public DecimalValidator(decimal? subject, string source, ValidatorMode mode = ValidatorMode.And)
        : base(subject, source, mode) {
        Connector = new Connector<decimal?, DecimalValidator>(this);
        _commandFactory = ValidationCommandFactory.For(typeof(decimal?), Source);
    }

    public IConnector<DecimalValidator> Connector { get; }

    public IConnector<DecimalValidator> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DecimalValidator> IsNotNull() {
        Negate();
        return IsNull();
    }

    public IConnector<DecimalValidator> MinimumIs(decimal minimum) {
        Negate();
        return IsLessThan(minimum);
    }

    public IConnector<DecimalValidator> IsGreaterThan(decimal minimum) {
        var validator = _commandFactory.Create(nameof(IsGreaterThan), minimum);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DecimalValidator> IsEqualTo(decimal value) {
        var validator = _commandFactory.Create(nameof(IsEqualTo), value);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DecimalValidator> IsLessThan(decimal maximum) {
        var validator = _commandFactory.Create(nameof(IsLessThan), maximum);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DecimalValidator> MaximumIs(decimal maximum) {
        Negate();
        return IsGreaterThan(maximum);
    }
}