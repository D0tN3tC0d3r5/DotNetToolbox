namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class HelpCommand : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    protected HelpCommand(IHasChildren parent)
        : base(parent, "Help", "?") {
        _parent = parent;
        Description = "Display help information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new HelpBuilder(_parent, includeApplication: false);
        Output.Write(builder.Build());
        return SuccessTask();
    }
}
