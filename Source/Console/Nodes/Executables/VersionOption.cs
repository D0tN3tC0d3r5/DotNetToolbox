namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class VersionOption
    : Command<VersionOption> {
    public VersionOption(IHasChildren parent)
        : base(parent, "--version") {
        Description = "Display version information.";
    }

    protected override Result Execute() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, Application.Name, " v", Application.Version);
        builder.AppendLine();
        builder.AppendLine();
        Application.Output.Write(builder);
        return Success();
    }
}
