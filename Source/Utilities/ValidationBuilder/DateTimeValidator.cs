namespace DotNetToolbox.ValidationBuilder;

public class DateTimeValidator : Validator<DateTime?>, IDateTimeProviderValidator {
    private readonly ValidationCommandFactory _commandFactory;

    public DateTimeValidator(DateTime? subject, string source, ValidatorMode mode = And)
        : base(subject, source, mode) {
        Connector = new Connector<DateTime?, DateTimeValidator>(this);
        _commandFactory = ValidationCommandFactory.For(typeof(DateTime?), Source);
    }

    public IConnector<DateTimeValidator> Connector { get; }

    public IConnector<DateTimeValidator> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DateTimeValidator> IsNotNull() {
        Negate();
        return IsNull();
    }

    public IConnector<DateTimeValidator> IsBefore(DateTime dateTime) {
        var validator = _commandFactory.Create(nameof(IsBefore), dateTime);
        ValidateWith(validator);
        return Connector;
    }

    public IConnector<DateTimeValidator> StartsOn(DateTime dateTime) {
        Negate();
        return IsBefore(dateTime);
    }

    public IConnector<DateTimeValidator> EndsOn(DateTime dateTime) {
        Negate();
        return IsAfter(dateTime);
    }

    public IConnector<DateTimeValidator> IsAfter(DateTime dateTime) {
        var validator = _commandFactory.Create(nameof(IsAfter), dateTime);
        ValidateWith(validator);
        return Connector;
    }
}