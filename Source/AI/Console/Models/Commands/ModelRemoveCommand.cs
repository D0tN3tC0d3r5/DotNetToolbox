namespace AI.Sample.Models.Commands;

public class ModelRemoveCommand : Command<ModelRemoveCommand> {
    private readonly IModelHandler _handler;

    public ModelRemoveCommand(IHasChildren parent, IModelHandler handler)
        : base(parent, "Remove", ["delete", "del"]) {
        _handler = handler;
        Description = "Remove a model.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var model = this.EntitySelectionPrompt(_handler.List(), "remove", "Settings", m => m.Key, m => m.Name);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            Output.WriteLine();

            return Result.SuccessTask();
        }

        if (!Input.Confirm($"Are you sure you want to remove the model '{model.Name}' ({model.Key})?")) {
            Output.WriteLine();

            return Result.InvalidTask("Action cancelled.");
        }

        try {
            _handler.Remove(model.Key);
            Output.WriteLine($"[green]Settings with key '{model.Name}' removed successfully.[/]");
            Output.WriteLine();

            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error removing the model.");
            Output.WriteLine();

            return Result.ErrorTask(ex.Message);
        }
    }
}
