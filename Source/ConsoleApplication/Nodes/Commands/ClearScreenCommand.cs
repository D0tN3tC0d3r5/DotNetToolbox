namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IHasChildren parent)
        : base(parent, "ClearScreen", "cls") {
        Description = "Clear the screen.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        Output.ClearScreen();
        return SuccessTask();
    }
}
