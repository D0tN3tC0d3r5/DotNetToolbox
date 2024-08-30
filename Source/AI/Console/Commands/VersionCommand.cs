namespace AI.Sample.Commands;

public class VersionCommand : Command<VersionCommand> {
    public VersionCommand(IHasChildren parent)
        : base(parent, "Version") {
        Description = "Display the current version of Lola.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        AnsiConsole.MarkupLine($"[bold]Lola version:[/] {Application.Version}");

        return Task.FromResult(Result.Success());
    }
}
