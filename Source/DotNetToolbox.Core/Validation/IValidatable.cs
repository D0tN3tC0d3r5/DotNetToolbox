namespace System.Validation;

public interface IValidatable {
    IValidationResult Validate(IDictionary<string, object?>? context = null);
}
