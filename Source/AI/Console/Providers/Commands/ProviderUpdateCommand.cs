namespace AI.Sample.Providers.Commands;

public class ProviderUpdateCommand(IHasChildren parent, IProviderHandler providerHandler)
    : Command<ProviderUpdateCommand>(parent, "Update", ["edit"]) {
    private readonly IProviderHandler _providerHandler = providerHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var key = AnsiConsole.Ask<uint>("Enter the key of the provider to update:");
        var provider = _providerHandler.GetByKey(key);

        if (provider == null) {
            Output.WriteLine($"[red]Provider with key '{key}' not found.[/]");
            return Result.InvalidTask($"Provider with key '{key}' not found.");
        }

        provider.Name = AnsiConsole.Ask("Enter the new name for the provider:", provider.Name);

        try {
            _providerHandler.Update(provider);
            Output.WriteLine($"[green]Provider updated successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError(ex, "Error updating the provider.");
            return Result.InvalidTask(ex.Message);
        }
    }
}
