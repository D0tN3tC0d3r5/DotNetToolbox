namespace AI.Sample.Providers.Commands;

public class ProviderListCommand(IHasChildren parent, IProviderHandler providerHandler)
    : Command<ProviderListCommand>(parent, "List", ["ls"]) {
    private readonly IProviderHandler _providerHandler = providerHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var providers = _providerHandler.List();

        var table = new Table();
        table.AddColumn("Key");
        table.AddColumn("Name");

        foreach (var provider in providers) {
            table.AddRow(provider.Key.ToString(), provider.Name);
        }

        Output.Write(table);

        return Result.SuccessTask();
    }
}
