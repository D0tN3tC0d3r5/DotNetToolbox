namespace DotNetToolbox.ConsoleApplication.Commands;

internal class VersionCommand : Command<VersionCommand> {
    public VersionCommand(IHasChildren parent)
        : base(parent, "Version") {
        Description = "Display the application's version.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        Output.WriteLine((Parent as IApplication)!.FullName);
        return SuccessTask();
    }
}
