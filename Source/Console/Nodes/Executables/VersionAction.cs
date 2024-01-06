namespace ConsoleApplication.Nodes.Executables;

internal sealed class VersionAction : Action<VersionAction> {
    public VersionAction(IHasChildren parent)
        : base(parent, "--version") {
        Description = "Display version information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new StringBuilder();
        builder.AppendJoin(null, Application.Name, " v", Application.Version);
        builder.AppendLine();
        builder.AppendLine();
        Application.Output.Write(builder);
        return SuccessTask();
    }
}
