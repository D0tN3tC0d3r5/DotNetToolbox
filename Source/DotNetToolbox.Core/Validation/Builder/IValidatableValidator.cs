namespace System.Validation.Builder;

public interface IValidatableValidator : IValidator {
    IConnector<ValidatableValidator> IsNull();
    IConnector<ValidatableValidator> IsNotNull();
    IConnector<ValidatableValidator> IsValid();
}