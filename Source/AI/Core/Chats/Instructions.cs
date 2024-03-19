namespace DotNetToolbox.AI.Chats;

public class Instructions {
    public string[] Goals { get; set; } = [];
    public string[] AcceptanceCriteria { get; set; } = [];
    public string[] Requirements { get; set; } = [];
    public string[] Assumptions { get; set; } = [];
    public string[] Constraints { get; set; } = [];
    public List<Example> Examples { get; set; } = [];
    public List<Fact> Facts { get; set; } = [];

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendSection(Goals);
        builder.AppendSection(AcceptanceCriteria, "Acceptance Criteria");
        builder.AppendSection(Assumptions);
        builder.AppendSection(Requirements);
        builder.AppendSection(Constraints);
        builder.AppendSection(Examples);
        builder.AppendSection(Facts);
        return builder.ToString();
    }
}
