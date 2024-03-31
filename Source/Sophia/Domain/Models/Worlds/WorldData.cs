namespace Sophia.Models.Worlds;

public class WorldData()
    : IEntity<Guid> {
    private readonly IDateTimeProvider _dateTime = DateTimeProvider.Default;

    public WorldData(IDateTimeProvider dateTime)
        : this() {
        _dateTime = dateTime;
    }

    public Guid Id { get; set; }
    public DateTimeOffset DateTime => _dateTime.Now;
    public List<string> Facts { get; set; } = [];

    public World ToModel(IDateTimeProvider? dateTime = null) => new(dateTime) {
        Facts = Facts,
    };
}
