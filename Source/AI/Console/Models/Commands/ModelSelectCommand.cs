namespace AI.Sample.Models.Commands;

public class ModelSelectCommand(IHasChildren parent, IModelHandler handler)
    : Command<ModelSelectCommand>(parent, "Select", n => {
        n.Aliases = ["sel"];
        n.Description = "Select the default model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Select default model command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var models = handler.List();

        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models available. Please add a model before proceeding.[/]");
            Output.WriteLine();

            return Result.Success();
        }

        var selected = await Input.BuildSelectionPrompt<ModelEntity>("Select an model:")
                                    .AddChoices(models)
                                    .ConvertWith(c => c.Name)
                                    .ShowAsync(cts.Token);

        handler.Select(selected.Key);
        Output.WriteLine($"[green]Settings '{selected.Key}' selected successfully.[/]");
        Output.WriteLine();

        return Result.Success();
    }, "Error selecting a model.", ct);
}
