namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class VersionFlag
    : Flag<VersionFlag> {
    public VersionFlag(IHasChildren parent)
        : base(parent, "Version", []) {
        Description = "Display the application's version.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        Output.WriteLine((Parent as IApplication)!.FullName);
        return SuccessTask();
    }
}
