namespace AI.Sample.Providers.Commands;

public class ProviderListCommand(IHasChildren parent, IProviderHandler providerHandler)
    : Command<ProviderListCommand>(parent, "List", ["ls"]) {
    private readonly IProviderHandler _providerHandler = providerHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var providers = _providerHandler.List();

        if (providers.Length == 0) {
            Output.WriteLine("[yellow]No providers found.[/]");
            Logger.LogInformation("No providers found. List providers action cancelled.");
            return Result.SuccessTask();
        }

        var table = new Table();
        table.AddColumn(new TableColumn("[yellow]Id[/]"));
        table.AddColumn(new TableColumn("[yellow]Name[/]"));

        foreach (var provider in providers) {
            table.AddRow(provider.Key.ToString(), provider.Name);
        }

        Output.Write(table);
        Logger.LogInformation("Providers listed.");
        return Result.SuccessTask();
    }
}
