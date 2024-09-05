namespace DotNetToolbox.ConsoleApplication.Commands;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IHasChildren parent)
        : base(parent, "ClearScreen", ["cls"]) {
        Description = "Clear the screen.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) {
        Output.ClearScreen();
        return SuccessTask();
    }
}
