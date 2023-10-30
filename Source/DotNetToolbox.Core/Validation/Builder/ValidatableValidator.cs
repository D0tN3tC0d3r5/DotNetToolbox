namespace System.Validation.Builder;
public class ValidatableValidator : Validator<IValidatable?>, IValidatableValidator {
    private readonly ValidationCommandFactory _commandFactory;

    public ValidatableValidator(IValidatable? subject, string source, ValidatorMode mode = ValidatorMode.And)
        : base(subject, source, mode) {
        Connector = new Connector<IValidatable?, ValidatableValidator>(this);
        _commandFactory = ValidationCommandFactory.For(typeof(IValidatable), Source);
    }

    public IConnector<ValidatableValidator> Connector { get; }

    public IConnector<ValidatableValidator> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<ValidatableValidator> IsNotNull() {
        Negate();
        return IsNull();
    }

    public IConnector<ValidatableValidator> IsValid() {
        var validator = _commandFactory.Create(nameof(IsValid));
        ValidateWith(validator);
        return Connector;
    }
}