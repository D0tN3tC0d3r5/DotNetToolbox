namespace AI.Sample.Providers.Commands;

public class ProviderViewCommand(IHasChildren parent, Handlers.IProviderHandler handler)
    : Command<ProviderViewCommand>(parent, "Info", n => {
        n.Aliases = ["i"];
        n.Description = "View provider";
        n.Help = "Display the detailed information about a Provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers->View command...");
        var provider = await this.SelectEntityAsync<ProviderEntity, uint>(handler.List(), m => m.Name, lt);
        if (provider is null) {
            Logger.LogInformation("No provider selected.");
            return Result.Success();
        }

        Output.WriteLine("[yellow]Provider Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {provider.Name}");
        Output.WriteLine($"[blue]API Key:[/] {provider.ApiKey ?? "[red]Not Set[/]"}");
        return Result.Success();
    }, "Error displaying the provider.", ct);
}
