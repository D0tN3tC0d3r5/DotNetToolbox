namespace AI.Sample.Models.Commands;

public class ModelViewCommand : Command<ModelViewCommand> {
    private readonly IModelHandler _handler;
    private readonly IProviderHandler _providerHandler;

    public ModelViewCommand(IHasChildren parent, IModelHandler handler, IProviderHandler providerHandler)
        : base(parent, "Info", ["i"]) {
        _handler = handler;
        _providerHandler = providerHandler;
        Description = "Display detailed information about a model.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var model = await this.SelectEntityAsync(_handler.List(), "show", "Settings", m => m.Key, m => m.Name, ct);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            return Result.Success();
        }
        model.Provider = _providerHandler.GetByKey(model.ProviderKey)!;

        Output.WriteLine("[yellow]Model Information:[/]");
        Output.WriteLine($"[blue]Id:[/] {model.Key}{(model.Selected ? " [green](default)[/]" : "")}");
        Output.WriteLine($"[blue]Name:[/] {model.Name}");
        Output.WriteLine($"[blue]Provider:[/] {model.Provider!.Name}");
        Output.WriteLine($"[blue]Maximum Map Size:[/] {model.MaximumContextSize}");
        Output.WriteLine($"[blue]Maximum Output Tokens:[/] {model.MaximumOutputTokens}");
        Output.WriteLine($"[blue]Input Cost per MTok:[/] {model.InputCostPerMillionTokens:C}");
        Output.WriteLine($"[blue]Output Cost per MTok:[/] {model.OutputCostPerMillionTokens:C}");
        Output.WriteLine($"[blue]Training Date Cut-Off:[/] {model.TrainingDataCutOff:MMM yyyy}");
        Output.WriteLine();

        return Result.Success();
    }, "Error displaying the model information.", ct);
}
