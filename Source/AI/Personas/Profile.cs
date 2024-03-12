namespace DotNetToolbox.AI.Personas;

public class Profile {
    public string Description { get; init; } = "You are a helpful agent.";
    public string Personality { get; init; } = string.Empty;
    public List<Skill> Skills { get; set; } = [];
    public List<Information> CustomValues { get; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine(Description);
        if (!string.IsNullOrWhiteSpace(Personality)) builder.AppendLine(Personality);
        if (Skills.Count > 0) builder.AppendLine("Your skills are:");
        foreach (var skill in Skills)
            builder.AppendLine(skill.ToString());
        foreach (var information in CustomValues)
            builder.AppendLine(information.ToString());
        return builder.ToString();
    }
}
