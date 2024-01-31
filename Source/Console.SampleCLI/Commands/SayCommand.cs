namespace SampleCLI.Commands;

internal class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say", []) {
        AddParameter("Text");
    }

    public override Task<Result> Execute(CancellationToken ct) {
        var text = Application.Context["Text"];
        Environment.Output.WriteLine(text);
        return SuccessTask();
    }
}
