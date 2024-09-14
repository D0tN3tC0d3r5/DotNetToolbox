namespace AI.Sample.Providers.Commands;

public class ProviderUpdateCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderUpdateCommand>(parent, "Update", n => {
        n.Aliases = ["edit"];
        n.Description = "Update provider";
        n.Help = "Update a LLM provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers->Update command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var provider = await this.SelectEntityAsync(handler.List(), "update", "Provider", m => m.Key, m => m.Name, cts.Token);
        if (provider is null) {
            Logger.LogInformation("Provider updated action cancelled.");
            Output.WriteLine();

            return Result.Success();
        }

        provider.Name = await Input.BuildTextPrompt<string>("Enter the new name for the provider")
                                   .For("name").WithDefault(provider.Name)
                                   .AsRequired()
                                   .ShowAsync(cts.Token);

        handler.Update(provider);
        Output.WriteLine($"[green]Provider '{provider.Name}' updated successfully.[/]");
        Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' updated successfully.", provider.Key, provider.Name);
        Output.WriteLine();

        return Result.Success();
    }, "Error updating the provider.", ct);
}
