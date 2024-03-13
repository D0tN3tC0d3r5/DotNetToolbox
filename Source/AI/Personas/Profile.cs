namespace DotNetToolbox.AI.Personas;

public class Profile {
    public string Description { get; init; } = "You are a helpful agent.";
    public string Personality { get; init; } = string.Empty;
    public List<string> Instructions { get; set; } = [];
    public List<Information> CustomValues { get; } = [];
    public List<Example> Examples { get; set; } = [];
    public List<Skill> Skills { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine(Description);
        if (!string.IsNullOrWhiteSpace(Personality)) builder.AppendLine(Personality);
        if (Instructions.Count > 0) builder.AppendLine("Instructions:");
        foreach (var instruction in Instructions)
            builder.AppendLine(instruction);
        if (Examples.Count > 0) builder.AppendLine("Custom Values:");
        foreach (var value in CustomValues)
            builder.AppendLine(value.ToString());
        if (Examples.Count > 0) builder.AppendLine("Examples:");
        foreach (var example in Examples)
            builder.AppendLine(example.ToString());
        if (Skills.Count > 0) builder.AppendLine("Your skills are:");
        foreach (var skill in Skills)
            builder.AppendLine(skill.ToString());
        return builder.ToString();
    }
}
