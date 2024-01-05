namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

internal class HelpAction : Action<HelpAction> {
    private readonly IHasChildren _parent;

    protected HelpAction(IHasChildren parent)
        : base(parent, "--help", "-h") {
        _parent = parent;
        Description = "Display help information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new HelpBuilder(_parent, includeApplication: true);
        Output.Write(builder.Build());
        return SuccessTask();
    }
}
