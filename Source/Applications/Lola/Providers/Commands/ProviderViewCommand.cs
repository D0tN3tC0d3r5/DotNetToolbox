namespace Lola.Providers.Commands;

public class ProviderViewCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderViewCommand>(parent, "Info", n => {
        n.Aliases = ["i"];
        n.Description = "View provider";
        n.Help = "Display the detailed information about a Provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers->View command...");
        var providers = handler.List();
        if (providers.Length == 0) {
            Output.WriteLine("[yellow]No providers found.[/]");
            Logger.LogInformation("No providers found. View provider action cancelled.");
            return Result.Success();
        }

        var provider = await this.SelectEntityAsync<ProviderEntity, uint>(providers.OrderBy(p => p.Name), m => m.Name, lt);
        if (provider is null) {
            Logger.LogInformation("No provider selected.");
            return Result.Success();
        }

        ShowDetails(provider);
        return Result.Success();
    }, "Error displaying the provider.", ct);

    private void ShowDetails(ProviderEntity provider) {
        Output.WriteLine("[yellow]Provider Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {provider.Name}");
        Output.WriteLine($"[blue]API Key:[/] {provider.ApiKey ?? "[red]Not Set[/]"}");
    }
}
