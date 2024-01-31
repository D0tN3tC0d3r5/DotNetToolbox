namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpCommand
    : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", ["?"]) {
        _parent = parent;
        Description = "Display this help information.";
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var help = FormatHelp(_parent);
        Environment.Output.WriteLine(help);
        return SuccessTask();
    }
}
