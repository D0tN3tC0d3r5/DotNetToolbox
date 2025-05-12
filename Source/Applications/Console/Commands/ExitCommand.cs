namespace DotNetToolbox.ConsoleApplication.Commands;

public class ExitCommand(IHasChildren parent)
    : Command<ExitCommand>(parent, "Exit", static n => {
        n.Aliases = ["quit"];
        n.Description = "Exit the application";
        n.Help = "Exit the application.";
    }) {
    protected override Result Execute() {
        Application.Exit();
        return Success();
    }
}
