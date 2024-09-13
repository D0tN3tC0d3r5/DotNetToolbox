namespace DotNetToolbox.ConsoleApplication.Commands;

public class BackCommand(IHasChildren parent)
    : Command<BackCommand>(parent, "Back", n => n.Description = "Return to the previous menu.") {
    protected override Result Execute() => Success();
}
