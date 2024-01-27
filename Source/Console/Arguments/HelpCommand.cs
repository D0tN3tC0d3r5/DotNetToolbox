namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpCommand
    : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", "?") {
        _parent = parent;
        Description = "Display help information.";
    }

    protected override Task<Result> Execute() {
        var help = FormatHelp(_parent, includeApplication: false); ;
        Application.Output.Write(help);
        return SuccessTask();
    }
}
