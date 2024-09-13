using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Create", ["add", "new"]) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct) => this.HandleCommandAsync((ct) => {
        var provider = handler.Create(async p => await SetUpAsync(p, ct));
        handler.Add(provider);
        Output.WriteLine($"[green]Provider '{provider.Name}' added successfully.[/]");
        Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' added successfully.", provider.Key, provider.Name);
        Output.WriteLine();

        return Result.SuccessTask();
    }, "Error adding the new provider.", ct);

    private async Task SetUpAsync(ProviderEntity provider, CancellationToken ct)
        => provider.Name = await Input.BuildTextPrompt<string>("Enter the provider name:")
                                .For("name")
                                .AsRequired()
                                .ShowAsync(ct);
}
