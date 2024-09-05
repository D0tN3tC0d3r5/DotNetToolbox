namespace AI.Sample.Provider.Commands;

public class ProviderViewCommand : Command<ProviderViewCommand> {
    private readonly IProviderHandler _handler;

    public ProviderViewCommand(IHasChildren parent, IProviderHandler handler)
        : base(parent, "Info", ["i"]) {
        _handler = handler;
        Description = "Display the Provider.";
    }

    protected override Result Execute() {
        var provider = this.EntitySelectionPrompt(_handler.List(), "show", "Settings", m => m.Key, m => m.Name);
        if (provider is null) {
            Logger.LogInformation("No provider selected.");
            return Result.Success();
        }

        Output.WriteLine("[yellow]Provider Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {provider.Name}");
        Output.WriteLine($"[blue]API Key:[/] {provider.ApiKey ?? "[red]Not Set[/]"}");
        Output.WriteLine();

        return Result.Success();
    }
}
