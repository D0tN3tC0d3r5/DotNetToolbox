namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpFlag
    : Command<HelpFlag>, IFlag {
    public HelpFlag(IHasChildren parent)
        : base(parent, "Help", "h", "?") {
        Description = "Displays this help information and finishes.";
    }

    protected override Task<Result> Execute() {
        var help = FormatHelp(Parent, includeApplication: true); ;
        Application.Output.Write(help);
        return SuccessTask();
    }
}
