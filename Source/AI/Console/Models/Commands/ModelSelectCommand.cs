namespace AI.Sample.Models.Commands;

public class ModelSelectCommand : Command<ModelSelectCommand> {
    private readonly IModelHandler _handler;

    public ModelSelectCommand(IHasChildren parent, IModelHandler handler)
        : base(parent, "Select", ["sel"]) {
        _handler = handler;
        Description = "Select the default model.";
    }

    protected override Result Execute() {
        var models = _handler.List();

        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models available. Please add a model before proceeding.[/]");
            Output.WriteLine();

            return Result.Success();
        }

        var selected = Input.BuildSelectionPrompt<ModelEntity>("Select an model:")
                            .AddChoices(models)
                            .ConvertWith(c => c.Name)
                            .Show();

        try {
            _handler.Select(selected.Key);
            Output.WriteLine($"[green]Settings '{selected.Key}' selected successfully.[/]");
            Output.WriteLine();

            return Result.Success();
        }
        catch (Exception ex) {
            Output.WriteError("Error selecting an model.");
            Output.WriteLine();

            return Result.Error(ex);
        }
    }
}
