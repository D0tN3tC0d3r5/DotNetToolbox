namespace ConsoleApplication.Nodes.Executables;

internal class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IHasChildren parent)
        : base(parent, "Exit") {
        Description = "Exit the application.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        Application.Exit();
        return SuccessTask();
    }
}
