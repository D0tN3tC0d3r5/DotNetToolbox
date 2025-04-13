namespace WebApi.Options;

public record TokenOptions
    : IValidatable {
    public virtual Result Validate(IMap? context = null)
        => Result.Default;
}
