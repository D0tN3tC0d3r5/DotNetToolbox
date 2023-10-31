namespace DotNetToolbox.ValidationBuilder.Commands;

public interface IValidationCommand {
    Result Validate(object? subject);
    Result Negate(object? subject);
}
