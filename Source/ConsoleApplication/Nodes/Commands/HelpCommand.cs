namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class HelpCommand : Command<HelpCommand> {
    protected HelpCommand(IHasChildren parent)
        : base(parent, "Help", "?") {
        Description = "Display help information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new HelpBuilder(this, includeApplicationName: false);
        Output.Write(builder.Build());
        return SuccessTask();
    }
}
