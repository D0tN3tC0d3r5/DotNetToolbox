namespace DotNetToolbox.ValidationBuilder;

public interface IObjectValidator : IValidator {
    IConnector<ObjectValidator> IsNull();
    IConnector<ObjectValidator> IsNotNull();
}