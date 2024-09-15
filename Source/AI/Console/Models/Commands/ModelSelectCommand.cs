namespace AI.Sample.Models.Commands;

public class ModelSelectCommand(IHasChildren parent, IModelHandler handler)
    : Command<ModelSelectCommand>(parent, "Select", n => {
        n.Aliases = ["sel"];
        n.Description = "Select default model.";
        n.Help = "Select the default model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Select default model command...");
        var models = handler.List();
        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models available. Please add a model before proceeding.[/]");
            return Result.Success();
        }

        var selected = await Input.BuildSelectionPrompt<ModelEntity>("Select an model:")
                                  .AddChoices(models.OrderBy(m => m.ProviderKey).ThenBy(m => m.Name))
                                  .ConvertWith(c => c.Name)
                                  .ShowAsync(lt);

        handler.Select(selected.Key);
        Output.WriteLine($"[green]Settings '{selected.Key}' selected successfully.[/]");
        return Result.Success();
    }, "Error selecting a model.", ct);
}
