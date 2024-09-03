namespace AI.Sample.Models.Commands;

public class ModelSelectCommand(IHasChildren parent, IModelHandler modelHandler)
    : Command<ModelSelectCommand>(parent, "SelectionPrompt", ["sel"]) {
    private readonly IModelHandler _modelHandler = modelHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var models = _modelHandler.List();

        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models available. Please add an model first.[/]");
            return Result.SuccessTask();
        }

        var selected = Input.SelectionPrompt<ModelEntity>("Select an model:")
                            .AddChoices(models)
                            .ConvertWith(c => $"{c.Key}: {c.Name}")
                            .Show();

        try {
            _modelHandler.Select(selected.Key);
            Output.WriteLine($"[green]Settings '{selected.Key}' selected successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error selecting an model.");
            return Result.ErrorTask(ex.Message);
        }
    }
}
