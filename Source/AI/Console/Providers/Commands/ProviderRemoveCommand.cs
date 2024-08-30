namespace AI.Sample.Providers.Commands;

public class ProviderRemoveCommand(IHasChildren parent, IProviderHandler providerHandler)
    : Command<ProviderRemoveCommand>(parent, "Remove", ["delete", "del"]) {
    private readonly IProviderHandler _providerHandler = providerHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var key = AnsiConsole.Ask<uint>("Enter the key of the provider to remove:");
        if (!AnsiConsole.Confirm($"Are you sure you want to remove the provider with key '{key}'?"))
            return Result.InvalidTask("Action cancelled.");

        try {
            _providerHandler.Remove(key);
            Output.WriteLine($"[green]Provider with key '{key}' removed successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError(ex, "Error removing the provider.");
            return Result.ErrorTask(ex);
        }
    }
}
