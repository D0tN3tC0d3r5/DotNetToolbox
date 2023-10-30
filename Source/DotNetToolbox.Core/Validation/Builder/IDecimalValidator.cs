namespace System.Validation.Builder;

public interface IDecimalValidator : IValidator {
    IConnector<DecimalValidator> IsNull();
    IConnector<DecimalValidator> IsNotNull();
    IConnector<DecimalValidator> MinimumIs(decimal minimum);
    IConnector<DecimalValidator> IsGreaterThan(decimal minimum);
    IConnector<DecimalValidator> IsEqualTo(decimal value);
    IConnector<DecimalValidator> IsLessThan(decimal maximum);
    IConnector<DecimalValidator> MaximumIs(decimal maximum);
}