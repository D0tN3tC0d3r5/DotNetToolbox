namespace AI.Sample.Providers.Commands;

public class ProviderUpdateCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderUpdateCommand>(parent, "Update", ["edit"]) {
    private readonly IProviderHandler _handler = handler;

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var provider = await this.SelectEntityAsync(_handler.List(), "update", "Provider", m => m.Key, m => m.Name, ct);
        if (provider is null) {
            Logger.LogInformation("Provider updated action cancelled.");
            Output.WriteLine();

            return Result.Success();
        }

        provider.Name = await Input.BuildTextPrompt<string>("Enter the new name for the provider")
                                   .For("name").WithDefault(provider.Name)
                                   .AsRequired()
                                   .ShowAsync(ct);

        _handler.Update(provider);
        Output.WriteLine($"[green]Provider '{provider.Name}' updated successfully.[/]");
        Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' updated successfully.", provider.Key, provider.Name);
        Output.WriteLine();

        return Result.Success();
    }, "Error updating the provider.", ct);
}
