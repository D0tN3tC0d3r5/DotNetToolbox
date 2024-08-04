namespace DotNetToolbox.AI.Jobs;

public class World
    : Context, IValidatable {
    public World(IDateTimeProvider? dateTime = null, IGuidProvider? guid = null) {
        this[nameof(DateTimeProvider)] = dateTime ?? DateTimeProvider.Default;
        this[nameof(GuidProvider)] = guid ?? GuidProvider.Default;
    }

    public DateTimeOffset DateTime => ((IDateTimeProvider)this[nameof(DateTimeProvider)]!).Now;

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
