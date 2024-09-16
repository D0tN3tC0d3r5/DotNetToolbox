namespace Lola.Providers.Commands;

public class ProviderRemoveCommand(IHasChildren parent, IProviderHandler handler, IModelHandler modelHandler)
    : Command<ProviderRemoveCommand>(parent, "Remove", n => {
        n.Aliases = ["delete", "del"];
        n.Description = "Remove provider";
        n.Help = "Remove a LLM provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers->Remove command...");
        var result = await SelectProvider(handler, lt);
        if (!result.IsSuccess) return result;
        var provider = result.Value;

        var models = modelHandler.List(provider.Key);
        if (models.Length > 0) {
            Output.WriteLine("[yellow bold]The following model(s) will also be deleted.[/]");
            ShowList(models);
        }

        if (!Input.Confirm($"Are you sure you want to delete '{provider.Name}' ({provider.Key})?")) {
            Logger.LogInformation("Provider remove action cancelled.");
            return Result.Success();
        }

        handler.Remove(provider.Key);
        Output.WriteLine($"[green]Provider with key '{provider.Name}' removed successfully.[/]");
        Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' removed successfully.", provider.Key, provider.Name);
        return Result.Success();
    }, "Error removing the provider.", ct);

    private async Task<Result<ProviderEntity>> SelectProvider(IProviderHandler handler, CancellationToken lt) {
        var providers = handler.List();
        if (providers.Length == 0) {
            Output.WriteLine("[yellow]No LLM providers found.[/]");
            Logger.LogInformation("No LLM providers found.");
            return Result.Invalid<ProviderEntity>(new ValidationError("No LLM providers found."));
        }
        var provider = await this.SelectEntityAsync<ProviderEntity, uint>(providers.OrderBy(p => p.Name), m => m.Name, lt);
        if (provider is null) {
            Logger.LogInformation("No LLM provider selected.");
            return Result.Invalid<ProviderEntity>(new ValidationError("No LLM provider selected."));
        }
        return Result.Success(provider);
    }

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
