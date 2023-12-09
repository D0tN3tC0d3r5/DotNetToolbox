namespace DotNetToolbox.ValidationBuilder;

public interface IIntegerValidator : IValidator {
    IConnector<IntegerValidator> IsNull();
    IConnector<IntegerValidator> IsNotNull();
    IConnector<IntegerValidator> MinimumIs(int minimum);
    IConnector<IntegerValidator> IsGreaterThan(int minimum);
    IConnector<IntegerValidator> IsEqualTo(int value);
    IConnector<IntegerValidator> IsLessThan(int maximum);
    IConnector<IntegerValidator> MaximumIs(int maximum);
}