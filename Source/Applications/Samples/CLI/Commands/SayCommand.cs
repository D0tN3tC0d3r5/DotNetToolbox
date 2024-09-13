namespace SampleCLI.Commands;

internal sealed class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say") {
        AddParameter("Text");
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) {
        var text = Application.Map["Text"];
        Output.WriteLine(text);
        return SuccessTask();
    }
}
