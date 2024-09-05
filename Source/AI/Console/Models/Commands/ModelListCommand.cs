namespace AI.Sample.Models.Commands;

public class ModelListCommand : Command<ModelListCommand> {
    private readonly IModelHandler _modelHandler;
    private readonly IProviderHandler _providerHandler;

    public ModelListCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
        : base(parent, "List", ["ls"]) {
        _modelHandler = modelHandler;
        _providerHandler = providerHandler;
        Description = "List all models or models for a specific provider.";
        AddParameter("Provider", "");
    }

    protected override Result Execute() {
        var providerKeyStr = Context.GetValueOrDefault<string>("Provider");

        var models = string.IsNullOrEmpty(providerKeyStr)
            ? _modelHandler.List()
            : _modelHandler.ListByProvider(providerKeyStr);

        if (models.Length == 0) {
            Output.WriteLine("[yellow]No models found.[/]");
            Output.WriteLine();

            return Result.Success();
        }

        var sortedModels = models
                .OrderBy(m => m.Provider!.Name)
                .ThenBy(m => m.Name);

        var table = new Table();
        table.Expand();

        // Add columns
        table.AddColumn(new TableColumn("[yellow]Name[/]"));
        table.AddColumn(new TableColumn("[yellow]Provider[/]"));
        table.AddColumn(new TableColumn("[yellow]Id[/]"));
        table.AddColumn(new TableColumn("[yellow]Context Size[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]Output Tokens[/]").RightAligned());

        foreach (var model in sortedModels) {
            var provider = _providerHandler.GetByKey(model.ProviderKey)!;
            table.AddRow(
                model.Name,
                provider.Name,
                model.Key,
                $"{model.MaximumContextSize:#,##0}",
                $"{model.MaximumOutputTokens:#,##0}"
            );
        }

        Output.Write(table);
        Output.WriteLine();
        return Result.Success();
    }
}
