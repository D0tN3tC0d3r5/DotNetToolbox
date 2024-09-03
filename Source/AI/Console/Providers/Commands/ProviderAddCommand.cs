namespace AI.Sample.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Create", ["add", "new"]) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        try {
            var provider = handler.Create(SetUp);
            handler.Add(provider);
            Output.WriteLine($"[green]ProviderId '{provider.Name}' added successfully.[/]");
            Logger.LogInformation("ProviderId '{ProviderKey}:{ProviderName}' added successfully.", provider.Key, provider.Name);
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error adding the new provider.");
            Logger.LogError(ex, "Error adding the new provider.");
            return Result.ErrorTask(ex.Message);
        }
    }

    private void SetUp(ProviderEntity provider)
        => provider.Name = Input.TextPrompt("Enter the provider name:")
                                .For("name").AsRequired();
}
