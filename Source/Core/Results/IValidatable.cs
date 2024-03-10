namespace DotNetToolbox.Results;

public interface IValidatable {
    Result Validate(IDictionary<string, object?>? context = null);
}

public interface IValidatableAsync {
    Task<Result> Validate(IDictionary<string, object?>? context = null, CancellationToken token = default);
}
