namespace DotNetToolbox.Results;

public interface IValidatable {
    Result Validate(IMap? context = null);
}

public interface IValidatableAsync {
    Task<Result> Validate(IMap? context = null, CancellationToken token = default);
}
