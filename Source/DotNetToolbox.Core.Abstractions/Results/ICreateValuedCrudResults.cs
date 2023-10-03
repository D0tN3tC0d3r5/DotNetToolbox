using System.Validation;

namespace System.Results;

public interface ICreateValuedCrudResults<out TSelf, in TValue> {
    static abstract TSelf Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract TSelf Invalid(IValidationResult result);
    static abstract TSelf Invalid(IEnumerable<IValidationError> errors);
    static abstract TSelf Invalid(IValidationError error);
    static abstract TSelf NotFound();
    static abstract TSelf Success(TValue value);
    static abstract TSelf Conflict(TValue value);
}