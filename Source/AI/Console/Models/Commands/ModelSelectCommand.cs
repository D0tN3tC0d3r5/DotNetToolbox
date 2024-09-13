namespace AI.Sample.Models.Commands;

public class ModelSelectCommand : Command<ModelSelectCommand> {
    private readonly IModelHandler _handler;

    public ModelSelectCommand(IHasChildren parent, IModelHandler handler)
        : base(parent, "Select", ["sel"]) {
        _handler = handler;
        Description = "Select the default model.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var models = _handler.List();

        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models available. Please add a model before proceeding.[/]");
            Output.WriteLine();

            return Result.Success();
        }

        var selected = await Input.BuildSelectionPrompt<ModelEntity>("Select an model:")
                                    .AddChoices(models)
                                    .ConvertWith(c => c.Name)
                                    .ShowAsync(ct);

        _handler.Select(selected.Key);
        Output.WriteLine($"[green]Settings '{selected.Key}' selected successfully.[/]");
        Output.WriteLine();

        return Result.Success();
    }, "Error selecting a model.", ct);
}
