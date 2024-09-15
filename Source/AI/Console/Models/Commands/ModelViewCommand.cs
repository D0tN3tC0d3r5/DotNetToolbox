namespace AI.Sample.Models.Commands;

public class ModelViewCommand(IHasChildren parent, Handlers.IModelHandler handler, Providers.Handlers.IProviderHandler providerHandler)
    : Command<ModelViewCommand>(parent, "Info", n => {
        n.Aliases = ["i"];
        n.Description = "View model";
        n.Help = "Display detailed information about a model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Info command...");
        var models = handler.List();
        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models found.[/]");
            Logger.LogInformation("No models found. View model action cancelled.");
            return Result.Success();
        }
        var model = await this.SelectEntityAsync<ModelEntity, string>(models.OrderBy(m => m.ProviderKey).ThenBy(m => m.Name), m => m.Name, lt);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            return Result.Success();
        }
        model.Provider = providerHandler.GetByKey(model.ProviderKey)!;

        ShowDetails(model);

        return Result.Success();
    }, "Error displaying the model information.", ct);

    private void ShowDetails(ModelEntity model) {
        Output.WriteLine("[yellow]Model Information:[/]");
        Output.WriteLine($"[blue]Id:[/] {model.Key}{(model.Selected ? " [green](default)[/]" : "")}");
        Output.WriteLine($"[blue]Name:[/] {model.Name}");
        Output.WriteLine($"[blue]Provider:[/] {model.Provider!.Name}");
        Output.WriteLine($"[blue]Maximum Map Size:[/] {model.MaximumContextSize}");
        Output.WriteLine($"[blue]Maximum Output Tokens:[/] {model.MaximumOutputTokens}");
        Output.WriteLine($"[blue]Input Cost per MTok:[/] {model.InputCostPerMillionTokens:C}");
        Output.WriteLine($"[blue]Output Cost per MTok:[/] {model.OutputCostPerMillionTokens:C}");
        Output.WriteLine($"[blue]Training Date Cut-Off:[/] {model.TrainingDateCutOff:MMM yyyy}");
        Output.WriteLine();
    }
}
