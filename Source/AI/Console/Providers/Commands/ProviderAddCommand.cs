namespace AI.Sample.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Create", ["add", "new"]) {
    protected override Result Execute() {
        try {
            var provider = handler.Create(SetUp);
            handler.Add(provider);
            Output.WriteLine($"[green]Provider '{provider.Name}' added successfully.[/]");
            Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' added successfully.", provider.Key, provider.Name);
            Output.WriteLine();

            return Result.Success();
        }
        catch (Exception ex) {
            Output.WriteError("Error adding the new provider.");
            Logger.LogError(ex, "Error adding the new provider.");
            Output.WriteLine();

            return Result.Error(ex);
        }
    }

    private void SetUp(ProviderEntity provider)
        => provider.Name = Input.BuildTextPrompt<string>("Enter the provider name:")
                                .For("name").AsRequired();
}
