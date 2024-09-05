namespace AI.Sample.Providers.Commands;

public class ProviderUpdateCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderUpdateCommand>(parent, "Update", ["edit"]) {
    private readonly IProviderHandler _handler = handler;

    protected override Result Execute() {
        var provider = this.EntitySelectionPrompt(_handler.List(), "update", "Provider", m => m.Key, m => m.Name);
        if (provider is null) {
            Logger.LogInformation("Provider updated action cancelled.");
            Output.WriteLine();

            return Result.Success();
        }

        provider.Name = Input.BuildTextPrompt<string>("Enter the new name for the provider")
                             .For("name").WithDefault(provider.Name).AsRequired();

        try {
            _handler.Update(provider);
            Output.WriteLine($"[green]Provider '{provider.Name}' updated successfully.[/]");
            Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' updated successfully.", provider.Key, provider.Name);
            Output.WriteLine();

            return Result.Success();
        }
        catch (Exception ex) {
            Output.WriteError("Error updating the provider.");
            Logger.LogError(ex, "Error updating the provider '{ProviderKey}:{ProviderName}'.", provider.Key, provider.Name);
            Output.WriteLine();

            return Result.Invalid(ex.Message);
        }
    }
}
