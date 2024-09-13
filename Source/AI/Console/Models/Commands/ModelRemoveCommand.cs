namespace AI.Sample.Models.Commands;

public class ModelRemoveCommand : Command<ModelRemoveCommand> {
    private readonly IModelHandler _handler;

    public ModelRemoveCommand(IHasChildren parent, IModelHandler handler)
        : base(parent, "Remove", ["delete", "del"]) {
        _handler = handler;
        Description = "Remove a model.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var model = await this.SelectEntityAsync(_handler.List(), "remove", "Settings", m => m.Key, m => m.Name, ct);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            Output.WriteLine();

            return Result.Success();
        }

        if (!await Input.ConfirmAsync($"Are you sure you want to remove the model '{model.Name}' ({model.Key})?", ct)) {
            Output.WriteLine();

            return Result.Invalid("Action cancelled.");
        }

        _handler.Remove(model.Key);
        Output.WriteLine($"[green]Settings with key '{model.Name}' removed successfully.[/]");
        Output.WriteLine();
        return Result.Success();
    }, "Error removing a model.", ct);
}
