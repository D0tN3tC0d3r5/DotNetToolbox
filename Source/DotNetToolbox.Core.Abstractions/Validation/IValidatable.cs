namespace DotNetToolbox.Validation;

public interface IValidatable
{
    IValidationResult Validate(IDictionary<string, object?>? context = null);
}
