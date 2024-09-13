namespace DotNetToolbox.ConsoleApplication.Commands;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IHasChildren parent)
        : base(parent, "ClearScreen", ["cls"]) {
        Description = "Clear the screen.";
    }

    protected override Result Execute() {
        Output.ClearScreen();
        return Success();
    }
}
