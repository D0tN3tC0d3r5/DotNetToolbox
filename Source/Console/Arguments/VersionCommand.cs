namespace DotNetToolbox.ConsoleApplication.Arguments;

internal class VersionCommand : Command<VersionCommand> {
    public VersionCommand(IHasChildren parent)
        : base(parent, "Version", []) {
        Description = "Display the application's version.";
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        Application.Output.WriteLine(Application.FullName);
        return SuccessTask();
    }
}
