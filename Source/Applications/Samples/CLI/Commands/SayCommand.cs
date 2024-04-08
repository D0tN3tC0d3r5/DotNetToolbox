namespace SampleCLI.Commands;

internal sealed class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say", []) {
        AddParameter("Text");
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var text = Application.Context["Text"];
        Environment.ConsoleOutput.WriteLine(text);
        return SuccessTask();
    }
}
