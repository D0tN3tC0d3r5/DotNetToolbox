namespace AI.Sample.Main.Commands;

public class VersionCommand : Command<VersionCommand> {
    public VersionCommand(IHasChildren parent)
        : base(parent, "Version") {
        Description = "Display the current version of Lola.";
    }

    protected override Result Execute() => this.HandleCommand(() => {
        Output.WriteLine($"[bold]Lola version:[/] {Application.Version}");
        return Result.Success();
    }, "Error displaying the application version.");

}
