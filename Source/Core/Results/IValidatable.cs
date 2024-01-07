namespace DotNetToolbox.Results;

public interface IValidatable {
    Result Validate(IDictionary<string, object?>? context = null);
}
