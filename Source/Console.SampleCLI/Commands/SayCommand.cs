namespace SampleCLI.Commands;

internal class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren node)
        : base(node, "Say") {
        AddParameter("Text");
    }

    protected override Result Execute() {
        var text = Application.Data["Text"];
        Application.Output.WriteLine(text);
        return Success();
    }
}
