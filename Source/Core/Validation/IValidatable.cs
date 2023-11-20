namespace System.Validation;

public interface IValidatable {
    Result Validate(IDictionary<string, object?>? context = null);
}
