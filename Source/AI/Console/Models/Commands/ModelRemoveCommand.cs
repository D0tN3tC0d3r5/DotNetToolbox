namespace AI.Sample.Models.Commands;

public class ModelRemoveCommand(IHasChildren parent, IModelHandler handler)
    : Command<ModelRemoveCommand>(parent, "Remove", n => {
        n.Aliases = ["delete", "del"];
        n.Description = "Remove a model";
        n.Help = "Remove a model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Remove command...");
        var models = handler.List();
        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models found.[/]");
            Logger.LogInformation("No models found. Remove model action cancelled.");
            return Result.Success();
        }
        var model = await this.SelectEntityAsync<ModelEntity, string>(models.OrderBy(m => m.ProviderKey).ThenBy(m => m.Name), m => m.Name, lt);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            return Result.Success();
        }

        if (!await Input.ConfirmAsync($"Are you sure you want to remove the model '{model.Name}' ({model.Key})?", lt)) {
            return Result.Invalid("Action cancelled.");
        }

        handler.Remove(model.Key);
        Output.WriteLine($"[green]Settings with key '{model.Name}' removed successfully.[/]");
        return Result.Success();
    }, "Error removing a model.", ct);
}
