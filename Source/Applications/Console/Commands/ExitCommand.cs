namespace DotNetToolbox.ConsoleApplication.Commands;

public class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IHasChildren parent)
        : base(parent, "Exit", ["quit"]) {
        Description = "Exit the application.";
    }

    protected override Result Execute() {
        Application.Exit();
        return Success();
    }
}
