namespace DotNetToolbox.ValidationBuilder;

public interface IValidatableValidator : IValidator {
    IConnector<ValidatableValidator> IsNull();
    IConnector<ValidatableValidator> IsNotNull();
    IConnector<ValidatableValidator> IsValid();
}