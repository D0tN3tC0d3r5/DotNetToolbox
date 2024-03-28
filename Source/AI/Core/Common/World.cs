namespace DotNetToolbox.AI.Common;

public class World(IDateTimeProvider? dateTimeProvider = null)
    : IValidatable {
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider ?? new DateTimeProvider();
    public DateTimeOffset DateTime => DateTimeProvider.Now;
    public int Id { get; set; }
    public List<Fact> Facts { get; set; } = [];
    public List<Tool> AvailableTools { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.Append($"The current local date and time is {DateTime:f} ({DateTime.Offset.Hours:00}:{DateTime.Offset.Minutes:00}).");
        builder.AppendSection(Facts);
        return builder.ToString();
    }

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
