namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class VersionFlag
    : Command<VersionFlag>, IFlag {
    public VersionFlag(IHasChildren parent)
        : base(parent, "Version") {
        Description = "Displays the version and exits.";
    }

    protected override Task<Result> Execute() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, Application.Name, " v", Application.Version);
        builder.AppendLine();
        builder.AppendLine();
        Application.Output.Write(builder);
        return SuccessTask();
    }
}
