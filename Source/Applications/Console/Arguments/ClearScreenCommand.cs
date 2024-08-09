namespace DotNetToolbox.ConsoleApplication.Arguments;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IHasChildren parent)
        : base(parent, "ClearScreen", ["cls"]) {
        Description = "Clear the screen.";
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        Environment.OperatingSystem.Output.ClearScreen();
        return SuccessTask();
    }
}
