namespace DotNetToolbox.ConsoleApplication.Commands;

public class NullCommand : Command<NullCommand> {
    public NullCommand(IHasChildren parent)
        : base(parent, "Back", []) {
        Description = "Return to the previous menu.";
    }

    protected override Result Execute() => Success();
}
