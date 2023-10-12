namespace System.Validation;

public interface IValidatable {
    ValidationResult Validate(IDictionary<string, object?>? context = null);
}
