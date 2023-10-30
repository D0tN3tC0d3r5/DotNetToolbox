namespace System.Validation.Builder;

public interface ITypeValidator : IValidator {
    IConnector<TypeValidator> IsNull();
    IConnector<TypeValidator> IsNotNull();
    IConnector<TypeValidator> IsEqualTo<TType>();
}