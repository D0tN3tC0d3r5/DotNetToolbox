namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IHasChildren parent)
        : base(parent, "Exit") {
        Description = "Exit the application.";
    }

    protected override Task<Result> Execute() {
        Application.Exit();
        return SuccessTask();
    }
}
