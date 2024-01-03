namespace DotNetToolbox.ConsoleApplication.SampleShell.Commands;
internal class ClearCommand : Command<ClearCommand> {
    public ClearCommand(IApplication application)
        : base(application, "Clear") {
        Alias = "cls";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        Output.ClearScreen();
        return Result.SuccessTask();
    }
}
