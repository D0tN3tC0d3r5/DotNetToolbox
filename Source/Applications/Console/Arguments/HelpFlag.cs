namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpFlag
    : Flag<HelpFlag> {
    public HelpFlag(IHasChildren parent)
        : base(parent, "Help", ["h", "?"]) {
        Description = "Display this help information.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        Output.WriteLine(Parent.ToHelp());
        Application.Exit();
        return SuccessTask();
    }
}
