namespace DotNetToolbox.ConsoleApplication.Commands;

public class BackCommand(IHasChildren parent)
    : Command<BackCommand>(parent, "Back", static n => {
        n.Description = "Return to the previous menu.";
        n.Help = "Return to the previous menu.";
    }) {
    protected override Result Execute() => Success();
}
