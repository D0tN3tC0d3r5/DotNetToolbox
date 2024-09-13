namespace DotNetToolbox.ConsoleApplication.Commands;

public class ExitCommand(IHasChildren parent)
    : Command<ExitCommand>(parent, "Exit", n => {
        n.Aliases = ["quit"];
        n.Description = "Exit the application.";
    }) {
    protected override Result Execute() {
        Application.Exit();
        return Success();
    }
}
