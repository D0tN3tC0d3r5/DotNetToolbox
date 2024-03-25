namespace DotNetToolbox.AI.Common;

public class World(IDateTimeProvider? dateTimeProvider = null)
    : IValidatable {
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider ?? new DateTimeProvider();
    public DateTimeOffset DateTime => DateTimeProvider.Now;
    public int Id { get; set; }
    public string? Location { get; set; } = string.Empty;
    public UserProfile UserProfile { get; set; } = new();
    public List<Fact> Facts { get; set; } = [];
    public List<Tool> AvailableTools { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.Append($"The current local date and time is {DateTime:f} ({DateTime.Offset.Hours:00}:{DateTime.Offset.Minutes:00}).");
        builder.AppendIntoNewLine(GetAgentLocation(Location));
        builder.AppendIntoNewLine(UserProfile.ToString());
        builder.AppendSection(Facts);
        return builder.ToString();
    }

    private static string GetAgentLocation(string? location)
        => string.IsNullOrWhiteSpace(location)
               ? "You do not know your current location."
               : $"You are located at {location}.";

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
