namespace DotNetToolbox.ConsoleApplication.Commands;

internal class VersionCommand(IHasChildren parent)
    : Command<VersionCommand>(parent, "Version", n => n.Description = "Display the application's version.") {
    protected override Result Execute() {
        Output.WriteLine((Parent as IApplication)!.FullName);
        return Success();
    }
}
