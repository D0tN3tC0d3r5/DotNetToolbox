namespace DotNetToolbox.ValidationBuilder;

public interface IDateTimeProviderValidator : IValidator {
    IConnector<DateTimeValidator> IsNull();
    IConnector<DateTimeValidator> IsNotNull();
    IConnector<DateTimeValidator> IsAfter(DateTime dateTime);
    IConnector<DateTimeValidator> IsBefore(DateTime dateTime);
    IConnector<DateTimeValidator> StartsOn(DateTime dateTime);
    IConnector<DateTimeValidator> EndsOn(DateTime dateTime);
}
