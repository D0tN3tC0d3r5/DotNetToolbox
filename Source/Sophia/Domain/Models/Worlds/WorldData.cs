namespace Sophia.Models.Worlds;

public class WorldData() {
    private readonly IDateTimeProvider _dateTime = DateTimeProvider.Default;

    public WorldData(IDateTimeProvider dateTime)
        : this() {
        _dateTime = dateTime;
    }

    public DateTimeOffset DateTime => _dateTime.Now;
    [MaxLength(1000)]
    public string? Location { get; set; }

    public UserProfileData UserProfile { get; set; } = new();
    public List<FactData> Facts { get; set; } = [];
    public List<ToolData> Tools { get; set; } = [];

    public World ToModel(IDateTimeProvider? dateTime) => new(dateTime) {
        Location = Location,
        UserProfile = UserProfile?.ToModel() ?? new(),
        Facts = Facts.ToList(x => x.ToModel()),
        AvailableTools = Tools.ToList(x => x.ToModel()),
    };
}
