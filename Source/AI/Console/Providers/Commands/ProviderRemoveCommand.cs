namespace AI.Sample.Providers.Commands;

public class ProviderRemoveCommand(IHasChildren parent, IProviderHandler handler, IModelHandler modelHandler)
    : Command<ProviderRemoveCommand>(parent, "Remove", n => n.Aliases = ["delete", "del"]) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers->Remove command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var provider = await this.SelectEntityAsync(handler.List(), "remove", "Provider", m => m.Key, m => m.Name, cts.Token);
        if (provider is null) {
            Logger.LogInformation("Provider remove action cancelled.");
            Output.WriteLine();

            return Result.Success();
        }

        var associatedModels = modelHandler.ListByProvider($"{provider.Key}");

        if (associatedModels.Length > 0) {
            Output.WriteLine($"[yellow]The provider '{provider.Name}' has associated models:[/]");
            var table = new Table();
            table.AddColumn("Settings Key");
            table.AddColumn("Settings Name");
            foreach (var model in associatedModels) {
                table.AddRow(model.Key, model.Name);
            }
            Output.Write(table);
            Output.WriteLine("[yellow]Removing this provider will also remove all associated models.[/]");
        }

        if (!Input.Confirm($"Are you sure you want to remove the provider '{provider.Name}' ({provider.Key})?")) {
            Logger.LogInformation("Provider remove action cancelled.");
            return Result.Success();
        }

        handler.Remove(provider.Key);
        Output.WriteLine($"[green]Provider with key '{provider.Name}' removed successfully.[/]");
        Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' removed successfully.", provider.Key, provider.Name);
        Output.WriteLine();

        return Result.Success();
    }, "Error removing the provider.", ct);
}
