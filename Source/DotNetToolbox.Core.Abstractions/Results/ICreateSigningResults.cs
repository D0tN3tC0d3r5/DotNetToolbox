using System.Validation;

namespace System.Results;

public interface ICreateSignInResults<out TSelf> {
    static abstract TSelf Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract TSelf Invalid(IValidationResult result);
    static abstract TSelf Invalid(IEnumerable<ValidationError> errors);
    static abstract TSelf Invalid(ValidationError error);
    static abstract TSelf ConfirmationRequired(string token);
    static abstract TSelf TwoFactorRequired(string token);
    static abstract TSelf Success(string token);
    static abstract TSelf Blocked();
    static abstract TSelf Locked();
    static abstract TSelf Failure();
}
