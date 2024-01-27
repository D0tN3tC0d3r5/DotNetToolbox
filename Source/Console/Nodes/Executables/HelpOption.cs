namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class HelpOption
    : Command<HelpOption> {
    public HelpOption(IHasChildren parent)
        : base(parent, "--help", "-h", "-?") {
        Description = "Displays this help information and finishes.";
    }

    protected override Task<Result> Execute() {
        var help = FormatHelp(Parent, includeApplication: true); ;
        Application.Output.Write(help);
        return SuccessTask();
    }
}
