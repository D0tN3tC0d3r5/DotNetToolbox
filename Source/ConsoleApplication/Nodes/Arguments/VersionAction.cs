namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

internal class VersionAction : Action<VersionAction> {
    protected VersionAction(IHasChildren parent)
        : base(parent, "--version") {
        Description = "Display version information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new StringBuilder();
        builder.AppendJoin(null, Application.Name, " v", Application.Version);
        builder.AppendLine();
        builder.AppendLine();
        Output.Write(builder);
        return SuccessTask();
    }
}
