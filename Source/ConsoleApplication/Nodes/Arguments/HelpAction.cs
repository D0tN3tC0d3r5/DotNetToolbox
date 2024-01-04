namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

internal class HelpAction : Action<HelpAction> {
    protected HelpAction(IHasChildren parent)
        : base(parent, "--help", "-h") {
        Description = "Display help information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new HelpBuilder(this, includeApplicationName: true);
        Output.Write(builder.Build());
        return SuccessTask();
    }
}
