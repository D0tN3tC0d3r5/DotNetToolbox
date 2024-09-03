namespace AI.Sample.Providers.Commands;

public class ProviderUpdateCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderUpdateCommand>(parent, "Update", ["edit"]) {
    private readonly IProviderHandler _handler = handler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var provider = this.EntitySelectionPrompt(_handler.List(), "update", "ProviderId", m => m.Key, m => m.Name);
        if (provider is null) {
            Logger.LogInformation("ProviderId updated action cancelled.");
            return Result.SuccessTask();
        }

        provider.Name = Input.TextPrompt("Enter the new name for the provider")
                             .For("name").WithDefault(provider.Name).AsRequired();

        try {
            _handler.Update(provider);
            Output.WriteLine($"[green]ProviderId '{provider.Name}' updated successfully.[/]");
            Logger.LogInformation("ProviderId '{ProviderKey}:{ProviderName}' updated successfully.", provider.Key, provider.Name);
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error updating the provider.");
            Logger.LogError(ex, "Error updating the provider '{ProviderKey}:{ProviderName}'.", provider.Key, provider.Name);
            return Result.InvalidTask(ex.Message);
        }
    }
}
