namespace System.Validation.Commands;

public interface IValidationCommand {
    Result Validate(object? subject);
    Result Negate(object? subject);
}
