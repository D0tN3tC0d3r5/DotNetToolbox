namespace System.Validation.Builder;

public interface IObjectValidator : IValidator {
    IConnector<ObjectValidator> IsNull();
    IConnector<ObjectValidator> IsNotNull();
}