namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IApplication application)
        : base(application, "ClearScreen", "cls") {
        Description = "Clear the screen.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        Output.ClearScreen();
        return SuccessTask();
    }
}
