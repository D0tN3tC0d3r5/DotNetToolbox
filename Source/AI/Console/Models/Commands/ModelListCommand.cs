namespace AI.Sample.Models.Commands;

public class ModelListCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
    : Command<ModelListCommand>(parent, "List", n => {
        n.Aliases = ["ls"];
        n.Description = "List models.";
        n.Help = "List all the models or those from a specific LLM provider.";
        n.AddParameter("Provider", "The provider key to filter the models by.");
    }) {
    protected override Result Execute() => this.HandleCommand(() => {
        Logger.LogInformation("Executing Models->List command...");
        var providerKeyStr = Map.GetValueAs<string>("Provider");

        var models = string.IsNullOrEmpty(providerKeyStr)
            ? modelHandler.List()
            : modelHandler.ListByProvider(providerKeyStr);

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
        table.AddColumn(new("[yellow]Name[/]"));
        table.AddColumn(new("[yellow]Provider[/]"));
        table.AddColumn(new("[yellow]Id[/]"));
        table.AddColumn(new TableColumn("[yellow]Map Size[/]").RightAligned());
        table.AddColumn(new TableColumn("[yellow]Output Tokens[/]").RightAligned());

        foreach (var model in sortedModels) {
            var provider = providerHandler.GetByKey(model.ProviderKey)!;
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
    }, "Error listing models.");
}
