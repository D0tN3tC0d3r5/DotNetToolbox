namespace DotNetToolbox.ValidationBuilder;

public interface IStringValidator : IValidator {
    IConnector<StringValidator> IsNull();
    IConnector<StringValidator> IsNotNull();
    IConnector<StringValidator> IsEmpty();
    IConnector<StringValidator> IsNotEmpty();
    IConnector<StringValidator> IsEmptyOrWhiteSpace();
    IConnector<StringValidator> IsNotEmptyOrWhiteSpace();
    IConnector<StringValidator> LengthIsAtLeast(int length);
    IConnector<StringValidator> LengthIsAtMost(int length);
    IConnector<StringValidator> LengthIs(int length);
    IConnector<StringValidator> IsIn(params string[] list);
    IConnector<StringValidator> IsEmail();
    IConnector<StringValidator> IsPassword(IPasswordPolicy policy);
}
