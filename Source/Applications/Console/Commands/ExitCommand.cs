namespace DotNetToolbox.ConsoleApplication.Commands;

public class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IHasChildren parent)
        : base(parent, "Exit", ["quit"]) {
        Description = "Exit the application.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        Application.Exit();
        return SuccessTask();
    }
}
