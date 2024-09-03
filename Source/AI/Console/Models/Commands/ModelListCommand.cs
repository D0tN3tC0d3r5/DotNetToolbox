namespace AI.Sample.Models.Commands;

public class ModelListCommand : Command<ModelListCommand> {
    private readonly IModelHandler _modelHandler;
    private readonly IProviderHandler _providerHandler;

    public ModelListCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
        : base(parent, "List", ["ls"]) {
        _modelHandler = modelHandler;
        _providerHandler = providerHandler;
        Description = "List all models or models for a specific provider.";
        AddParameter("ProviderId", "");
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var providerKeyStr = Context.GetValueOrDefault<string>("ProviderId");

        var models = string.IsNullOrEmpty(providerKeyStr)
            ? _modelHandler.List()
            : _modelHandler.ListByProvider(providerKeyStr);

        var sortedModels = models.OrderBy(m => m.ProviderKey).ThenBy(m => m.Name);

        var table = new Table();
        table.Expand();

        // Add columns
        table.AddColumn(new TableColumn("[yellow]ProviderId[/]"));
        table.AddColumn(new TableColumn("[yellow]Name[/]"));
        table.AddColumn(new TableColumn("[yellow]Id[/]"));
        table.AddColumn(new TableColumn("[yellow]Context Size[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]Output Tokens[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]Input Cost per MTok[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]Output Cost per MTok[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]Training Date Cut-Off[/]").Centered());

        foreach (var model in sortedModels) {
            var provider = _providerHandler.GetByKey(model.ProviderKey)!;
            table.AddRow(
                provider.Name,
                model.Name,
                model.Key,
                $"{model.MaximumContextSize:#,##0}",
                $"{model.MaximumOutputTokens:#,##0}",
                $"{model.InputCostPerMillionTokens:$#,##0.00}",
                $"{model.OutputCostPerMillionTokens:$#,##0.00}",
                $"{model.TrainingDataCutOff:MMM yyyy}"
            );
        }

        Output.Write(table);
        return Result.SuccessTask();
    }
}
