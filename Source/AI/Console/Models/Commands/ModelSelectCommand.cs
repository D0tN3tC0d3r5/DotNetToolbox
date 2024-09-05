namespace AI.Sample.Models.Commands;

public class ModelSelectCommand : Command<ModelSelectCommand> {
    private readonly IModelHandler _handler;

    public ModelSelectCommand(IHasChildren parent, IModelHandler handler)
        : base(parent, "Select", ["sel"]) {
        _handler = handler;
        Description = "Select the default model.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var models = _handler.List();

        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models available. Please add a model before proceeding.[/]");
            Output.WriteLine();

            return Result.SuccessTask();
        }

        var selected = Input.BuildSelectionPrompt<ModelEntity>("Select an model:")
                            .AddChoices(models)
                            .ConvertWith(c => c.Name)
                            .Show();

        try {
            _handler.Select(selected.Key);
            Output.WriteLine($"[green]Settings '{selected.Key}' selected successfully.[/]");
            Output.WriteLine();

            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error selecting an model.");
            Output.WriteLine();

            return Result.ErrorTask(ex.Message);
        }
    }
}
