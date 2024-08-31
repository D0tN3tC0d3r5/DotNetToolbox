namespace AI.Sample.Providers.Commands;

public class ProviderRemoveCommand(IHasChildren parent, IProviderHandler handler, IModelHandler modelHandler)
    : Command<ProviderRemoveCommand>(parent, "Remove", ["delete", "del"]) {
    private readonly IProviderHandler _handler = handler;
    private readonly IModelHandler _modelHandler = modelHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var provider = this.EntitySelectionPrompt(_handler.List(), "remove", "Provider", m => m.Key, m => m.Name);
        if (provider is null) {
            Logger.LogInformation("Provider remove action cancelled.");
            return Result.SuccessTask();
        }

        var associatedModels = _modelHandler.ListByProvider($"{provider.Key}");

        if (associatedModels.Length > 0) {
            Output.WriteLine($"[yellow]The provider '{provider.Name}' has associated models:[/]");
            var table = new Table();
            table.AddColumn("Model Key");
            table.AddColumn("Model Name");
            foreach (var model in associatedModels) {
                table.AddRow(model.Key, model.Name);
            }
            Output.Write(table);
            Output.WriteLine("[yellow]Removing this provider will also remove all associated models.[/]");
        }

        if (!Input.Confirm($"Are you sure you want to remove the provider '{provider.Name}' ({provider.Key})?")) {
            Logger.LogInformation("Provider remove action cancelled.");
            return Result.SuccessTask();
        }

        try {
            _handler.Remove(provider.Key);
            Output.WriteLine($"[green]Provider with key '{provider.Name}' removed successfully.[/]");
            Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' removed successfully.", provider.Key, provider.Name);
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error removing the provider.");
            Logger.LogError(ex, "Error removing the provider '{ProviderKey}:{ProviderName}'.", provider.Key, provider.Name);
            return Result.ErrorTask(ex);
        }
    }
}
