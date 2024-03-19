using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.Personas;

public class Profile {
    public string Description { get; init; } = "You are a helpful agent.";
    public string Personality { get; init; } = string.Empty;
    public List<string> Instructions { get; set; } = [];
    public List<Information> AdditionalInformation { get; } = [];
    public List<Tool> KnownTools { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine(Description);
        if (!string.IsNullOrWhiteSpace(Personality)) builder.AppendLine(Personality);
        if (KnownTools.Count > 0) builder.AppendLine("Your skills are:");
        foreach (var tool in KnownTools)
            builder.AppendLine(tool.ToString());
        if (AdditionalInformation.Count > 0) builder.AppendLine("Additional information:");
        foreach (var information in AdditionalInformation)
            builder.AppendLine(information.ToString());
        if (Instructions.Count > 0) builder.AppendLine("Instructions:");
        foreach (var instruction in Instructions)
            builder.AppendLine(instruction);
        return builder.ToString();
    }
}
