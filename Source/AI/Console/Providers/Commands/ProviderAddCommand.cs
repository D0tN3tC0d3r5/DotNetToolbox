using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Add", n => {
        n.Aliases = ["new"];
        n.Description = "Add a new LLM provider.";
        n.Help = "Register a new LLM provider to use with your AI agents.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(lt => {
        Logger.LogInformation("Executing Providers->Add command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var provider = handler.Create(p => SetUpAsync(p, cts.Token).GetAwaiter().GetResult());
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
