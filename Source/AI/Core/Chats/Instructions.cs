namespace DotNetToolbox.AI.Chats;

public class Instructions {
    public HashSet<string> Goals { get; set; } = [];
    public HashSet<string> Requirements { get; set; } = [];
    public HashSet<string> Assumptions { get; set; } = [];
    public HashSet<string> Constraints { get; set; } = [];
    public HashSet<string> Examples { get; set; } = [];
    public HashSet<string> Validation { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendSection(Goals);
        builder.AppendSection(Assumptions);
        builder.AppendSection(Requirements);
        builder.AppendSection(Constraints);
        builder.AppendSection(Examples);
        builder.AppendSection(Validation);
        return builder.ToString();
    }
}
