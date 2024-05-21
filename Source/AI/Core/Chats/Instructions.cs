namespace DotNetToolbox.AI.Chats;

public class Instructions {
    public List<string> Goals { get; set; } = [];
    public List<string> Scope { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Assumptions { get; set; } = [];
    public List<string> Constraints { get; set; } = [];
    public List<string> Examples { get; set; } = [];
    public List<string> Strategy { get; set; } = [];
    public List<string> Validation { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendSection(Goals);
        builder.AppendSection(Scope);
        builder.AppendSection(Assumptions);
        builder.AppendSection(Requirements);
        builder.AppendSection(Constraints);
        builder.AppendSection(Examples);
        builder.AppendSection(Strategy);
        builder.AppendSection(Validation);
        return builder.ToString();
    }
}
