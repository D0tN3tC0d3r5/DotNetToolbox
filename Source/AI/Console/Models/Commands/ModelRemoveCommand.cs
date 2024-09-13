namespace AI.Sample.Models.Commands;

public class ModelRemoveCommand(IHasChildren parent, IModelHandler handler)
    : Command<ModelRemoveCommand>(parent, "Remove", n => {
        n.Aliases = ["delete", "del"];
        n.Description = "Remove a model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Remove command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var model = await this.SelectEntityAsync(handler.List(), "remove", "Settings", m => m.Key, m => m.Name, cts.Token);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            Output.WriteLine();

            return Result.Success();
        }

        if (!await Input.ConfirmAsync($"Are you sure you want to remove the model '{model.Name}' ({model.Key})?", cts.Token)) {
            Output.WriteLine();

            return Result.Invalid("Action cancelled.");
        }

        handler.Remove(model.Key);
        Output.WriteLine($"[green]Settings with key '{model.Name}' removed successfully.[/]");
        Output.WriteLine();
        return Result.Success();
    }, "Error removing a model.", ct);
}
