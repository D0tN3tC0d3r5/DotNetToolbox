namespace AI.Sample.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Add", ["new"]) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        var provider = handler.Create(p => p.Name = AnsiConsole.Ask<string>("Enter the provider name:"));
        try {
            handler.Add(provider);
            Output.WriteLine($"[green]Provider '{provider.Name}' added successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError(ex, "Error adding an provider.");
            return Result.ErrorTask(ex.Message);
        }
    }
}
