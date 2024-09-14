namespace AI.Sample.Providers.Commands;

public class ProviderRemoveCommand(IHasChildren parent, Handlers.IProviderHandler handler, Models.Handlers.IModelHandler modelHandler)
    : Command<ProviderRemoveCommand>(parent, "Remove", n => {
        n.Aliases = ["delete", "del"];
        n.Description = "Remove provider";
        n.Help = "Remove a LLM provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers->Remove command...");
        var provider = await this.SelectEntityAsync<ProviderEntity, uint>(handler.List(), m => m.Name, lt);
        if (provider is null) {
            Logger.LogInformation("Provider remove action cancelled.");
            return Result.Success();
        }

        var models = modelHandler.List(provider.Key);
        if (models.Length > 0) {
            Output.WriteLine($"[yellow bold]The following models will also be deleted.[/]");
            ShowList(models);
        }

        if (!Input.Confirm($"Are you sure you want to remove the provider '{provider.Name}' ({provider.Key})?")) {
            Logger.LogInformation("Provider remove action cancelled.");
            return Result.Success();
        }

        handler.Remove(provider.Key);
        Output.WriteLine($"[green]Provider with key '{provider.Name}' removed successfully.[/]");
        Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' removed successfully.", provider.Key, provider.Name);
        return Result.Success();
    }, "Error removing the provider.", ct);

    private void ShowList(ModelEntity[] models) {
        var table = new Table();
        table.AddColumn("Settings Key");
        table.AddColumn("Settings Name");
        foreach (var model in models) {
            table.AddRow(model.Key, model.Name);
        }
        Output.Write(table);
    }
}
