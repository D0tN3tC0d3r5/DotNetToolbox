namespace AI.Sample.Models.Commands;

public class ModelRemoveCommand : Command<ModelRemoveCommand> {
    private readonly IModelHandler _handler;

    public ModelRemoveCommand(IHasChildren parent, IModelHandler handler)
        : base(parent, "Remove", ["delete", "del"]) {
        _handler = handler;
        Description = "Remove a model.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var model = this.EntitySelectionPrompt(_handler.List(), "remove", "Model", m => m.Key, m => m.Name);
        if (model is null) {
            Logger.LogInformation("Model delete action cancelled.");
            return Result.SuccessTask();
        }

        if (!Input.Confirm($"Are you sure you want to remove the model '{model.Name}' ({model.Key})?")) {
            return Result.InvalidTask("Action cancelled.");
        }

        try {
            _handler.Remove(model.Key);
            Output.WriteLine($"[green]Model with key '{model.Name}' removed successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error removing the model.");
            return Result.ErrorTask(ex.Message);
        }
    }
}
