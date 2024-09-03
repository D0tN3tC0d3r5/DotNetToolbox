namespace AI.Sample.Models.Commands;

public class ModelInfoCommand : Command<ModelInfoCommand> {
    private readonly IModelHandler _handler;
    private readonly IProviderHandler _providerHandler;

    public ModelInfoCommand(IHasChildren parent, IModelHandler handler, IProviderHandler providerHandler)
        : base(parent, "Info", ["i"]) {
        _handler = handler;
        _providerHandler = providerHandler;
        Description = "Display detailed information about a model.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var model = this.EntitySelectionPrompt(_handler.List(), "show", "Settings", m => m.Key, m => m.Name);
        if (model is null) {
            Logger.LogInformation("No model selected. Settings display action cancelled.");
            return Result.SuccessTask();
        }

        var provider = _providerHandler.GetByKey(model.ProviderKey)!;

        var table = new Table();
        table.RoundedBorder().Title($"Settings Information: [yellow]{model.Name}[/]");
        table.AddColumn("[blue]Property[/]");
        table.AddColumn("[blue]Value[/]");

        table.AddRow("Id", model.Key);
        table.AddRow("Name", model.Name);
        table.AddRow("ProviderId", provider.Name);
        table.AddRow("Maximum Context Size", model.MaximumContextSize.ToString("#,##0").PadLeft(30));
        table.AddRow("Maximum Output Tokens", model.MaximumOutputTokens.ToString("#,##0").PadLeft(30));
        table.AddRow("Input Cost per MTok", model.InputCostPerMillionTokens.ToString("$#,##0.00").PadLeft(30));
        table.AddRow("Output Cost per MTok", model.OutputCostPerMillionTokens.ToString("$#,##0.00").PadLeft(30));
        table.AddRow("Training Date Cut-Off", $"{model.TrainingDataCutOff:MMM yyyy}");

        Output.Write(table);
        return Result.SuccessTask();
    }
}
