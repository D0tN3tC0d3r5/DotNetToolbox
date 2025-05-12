namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class VersionFlag(IHasChildren parent)
    : Flag<VersionFlag>(parent, "Version", static n => {
        n.Description = "Display the application's version.";
        n.Help = "Display the application's current version.";
    }) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        Output.WriteLine((Parent as IApplication)!.FullName);
        return Task.FromResult(Success());
    }
}
