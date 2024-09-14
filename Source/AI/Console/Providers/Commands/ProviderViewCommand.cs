namespace AI.Sample.Providers.Commands;

public class ProviderViewCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderViewCommand>(parent, "Info", n => {
        n.Aliases = ["i"];
        n.Description = "View provider";
        n.Help = "Display the detailed information about a Provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var provider = await this.SelectEntityAsync(handler.List(), "show", "Settings", m => m.Key, m => m.Name, cts.Token);
        if (provider is null) {
            Logger.LogInformation("No provider selected.");
            return Result.Success();
        }

        Output.WriteLine("[yellow]Provider Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {provider.Name}");
        Output.WriteLine($"[blue]API Key:[/] {provider.ApiKey ?? "[red]Not Set[/]"}");
        Output.WriteLine();

        return Result.Success();
    }, "Error displaying the provider.", ct);
}
