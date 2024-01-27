namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class ClearScreenOption
    : Command<ClearScreenOption> {
    public ClearScreenOption(IHasChildren parent)
        : base(parent, "--clearScreen", "-cls") {
        Description = "Clear the screen before the application starts.";
    }

    protected override Task<Result> Execute() {
        Application.Output.ClearScreen();
        return SuccessTask();
    }
}
