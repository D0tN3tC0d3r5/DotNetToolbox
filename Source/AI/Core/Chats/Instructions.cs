using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.Chats;

public class Instructions {
    public string Goal { get; set; } = string.Empty;
    public IList<Information> AdditionalInformation { get; } = [];
    public List<Example> Examples { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendLine(Goal);
        if (Examples.Count > 0) builder.AppendLine("Examples:");
        foreach (var example in Examples)
            builder.AppendLine(example.ToString());
        if (AdditionalInformation.Count > 0) builder.AppendLine("Additional information:");
        foreach (var value in AdditionalInformation)
            builder.AppendLine(value.ToString());
        return builder.ToString();
    }
}
