namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IHasChildren parent)
        : base(parent, "ClearScreen", "cls") {
        Description = "Clear the screen.";
    }

    protected override Task<Result> Execute() {
        Application.Output.ClearScreen();
        return SuccessTask();
    }
}
