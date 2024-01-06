namespace SampleCLI.Commands;

internal class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say") {
        AddParameter("Text");
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var text = Application.Data["Text"];
        Application.Output.WriteLine(text);
        return SuccessTask();
    }
}
