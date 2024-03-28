namespace Sophia.Models.Worlds;

public class WorldData() {
    private readonly IDateTimeProvider _dateTime = DateTimeProvider.Default;

    public WorldData(IDateTimeProvider dateTime)
        : this() {
        _dateTime = dateTime;
    }

    public DateTimeOffset DateTime => _dateTime.Now;
    public List<FactData> Facts { get; set; } = [];
    public List<ToolData> Tools { get; set; } = [];

    public World ToModel(IDateTimeProvider? dateTime = null) => new(dateTime) {
        Facts = Facts.ToList(x => x.ToModel()),
        AvailableTools = Tools.ToList(x => x.ToModel()),
    };
}
