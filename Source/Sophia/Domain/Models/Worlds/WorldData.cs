namespace Sophia.Models.Worlds;

public class WorldData() {
    private readonly IDateTimeProvider _dateTime = DateTimeProvider.Default;

    public WorldData(IDateTimeProvider dateTime)
        : this() {
        _dateTime = dateTime;
    }

    public DateTimeOffset DateTime => _dateTime.Now;
    public HashSet<string> Facts { get; set; } = [];

    public World ToModel(IDateTimeProvider? dateTime = null) => new(dateTime) {
        Facts = Facts,
    };
}
