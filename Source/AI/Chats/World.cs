namespace DotNetToolbox.AI.Chats;

public class World(IEnvironment environment) {
    public DateTimeOffset DateTime => environment.DateTime.Now;
    public string? Location { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<Information> CustomValues { get; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine($"The current date time is {DateTime}.");
        if (string.IsNullOrWhiteSpace(Location)) builder.AppendLine("You do not know your current location.");
        else builder.AppendLine($"You are located at '{Location}'.");
        if (string.IsNullOrWhiteSpace(UserName)) builder.AppendLine("You do not know the name of the user.");
        else builder.AppendLine($"The name of the user is '{Location}'.");
        foreach (var information in CustomValues)
            builder.AppendLine(information.ToString());
        return builder.ToString();
    }
}
