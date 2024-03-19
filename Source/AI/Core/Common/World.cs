namespace DotNetToolbox.AI.Common;

public class World(IDateTimeProvider? dateTime = null)
    : IValidatable {
    public DateTimeOffset DateTime => dateTime?.Now ?? DateTimeOffset.Now;
    public int Id { get; set; }
    public string? Location { get; set; } = string.Empty;
    public string? UserProfile { get; set; } = string.Empty;
    public List<Information> AdditionalInformation { get; set; } = [];
    public List<Tool> AvailableTools { get; set; } = [];

    public IDateTimeProvider? GetProvider() => dateTime;

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine($"The current date time is {DateTime}.");
        if (string.IsNullOrWhiteSpace(Location)) builder.AppendLine("You do not know your current location.");
        else builder.AppendLine($"You are located at '{Location}'.");
        if (string.IsNullOrWhiteSpace(UserProfile)) builder.AppendLine("You do not know the name of the user.");
        else builder.AppendLine($"The name of the user is '{Location}'.");
        if (AdditionalInformation.Count > 0) builder.AppendLine("Additional information;");
        foreach (var information in AdditionalInformation)
            builder.AppendLine(information.ToString());
        return builder.ToString();
    }

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
