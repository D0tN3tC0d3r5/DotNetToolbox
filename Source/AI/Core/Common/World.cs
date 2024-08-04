namespace DotNetToolbox.AI.Common;

public class World(IDateTimeProvider? dateTimeProvider = null)
    : IValidatable, IContextSection {
    public string Title { get; } = "General Facts";
    public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider ?? new DateTimeProvider();
    public DateTimeOffset DateTime => DateTimeProvider.Now;
    public int Id { get; set; }
    public UserProfile UserProfile { get; set; } = default!;
    public List<string> Facts { get; set; } = [];

    public string GetIndentedText(string indent) {
        var builder = new StringBuilder();
        builder.Append($"{indent}The current local date and time is {DateTime:f} ({DateTime.Offset.Hours:00}:{DateTime.Offset.Minutes:00}).");
        builder.AppendSection(indent, Facts);
        builder.AppendSection(indent, UserProfile);
        return builder.ToString();
    }

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
