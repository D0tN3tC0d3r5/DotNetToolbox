using DotNetToolbox.Validation;

namespace DotNetToolbox.Results;

public interface ICreateHttpResults<out TSelf>
{
    static abstract TSelf Ok();
    static abstract TSelf Created();
    static abstract TSelf BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract TSelf BadRequest(IValidationResult result);
    static abstract TSelf BadRequest(IEnumerable<IValidationError> errors);
    static abstract TSelf BadRequest(IValidationError error);
    static abstract TSelf Unauthorized();
    static abstract TSelf NotFound();
    static abstract TSelf Conflict();
}