namespace DotNetToolbox.ConsoleApplication.Commands;

internal class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IHasChildren parent)
        : base(parent, "Exit", []) {
        Description = "Exit the application.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        Application.Exit();
        return SuccessTask();
    }
}
