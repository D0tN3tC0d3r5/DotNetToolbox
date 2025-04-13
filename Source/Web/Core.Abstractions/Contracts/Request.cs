namespace WebApi.Contracts;

public abstract record Request
    : IValidatable {
    public virtual Result Validate(IMap? context = null)
        => Result.Success();
}
