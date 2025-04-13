namespace WebApi.Options;

public class SecretOptions : IValidatable {
    public const byte MinimumSize = 32;
    public const byte DefaultSize = MinimumSize;

    public byte Size {
        get;
        set => field = value switch {
            0 => DefaultSize,
            < 32 => MinimumSize,
            _ => value,
        };
    } = DefaultSize;

    public Result Validate(IMap? context = null) => Result.Success();
}
