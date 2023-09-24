using DotNetToolbox.Validation;

namespace DotNetToolbox.Results;

public interface ICreateValidationResults<out TSelf>
{
    static abstract TSelf Failure([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract TSelf Failure(IValidationResult result);
    static abstract TSelf Failure(IEnumerable<IValidationError> errors);
    static abstract TSelf Failure(IValidationError error);
    static abstract TSelf Success();
}
