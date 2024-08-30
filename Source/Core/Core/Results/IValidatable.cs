namespace DotNetToolbox.Results;

public interface IValidatable {
    Result Validate(IContext? context = null);
}

public interface IValidatableAsync {
    Task<Result> Validate(IContext? context = null, CancellationToken token = default);
}
