using System.Validation;

namespace System.Results;

public interface ICreateCrudResults<out TSelf> {

    static abstract TSelf Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract TSelf Invalid(IValidationResult result);
    static abstract TSelf Invalid(IEnumerable<IValidationError> errors);
    static abstract TSelf Invalid(IValidationError error);
    static abstract TSelf Success();
    static abstract TSelf NotFound();
    static abstract TSelf Conflict();
}