using System.Validation;

namespace System.Results;

public interface ICreateValuedHttpResults<out TSelf, in TValue> {
    static abstract TSelf Ok(TValue value);
    static abstract TSelf Created(TValue value);
    static abstract TSelf BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract TSelf BadRequest(IValidationResult result);
    static abstract TSelf BadRequest(IEnumerable<ValidationError> errors);
    static abstract TSelf BadRequest(ValidationError error);
    static abstract TSelf Unauthorized();
    static abstract TSelf NotFound();
    static abstract TSelf Conflict(TValue value);
}