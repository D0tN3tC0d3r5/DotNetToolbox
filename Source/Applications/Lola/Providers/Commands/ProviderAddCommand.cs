using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Add", n => {
        n.Aliases = ["new"];
        n.Description = "Add provider";
        n.Help = "Register a new LLM provider to use with your AI agents.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing Providers->Add command...");
            var provider = new ProviderEntity();
            await SetUpAsync(provider, lt);
            handler.Add(provider);
            Output.WriteLine($"[green]Provider '{provider.Name}' added successfully.[/]");
            Logger.LogInformation("Provider '{ProviderId}:{ProviderName}' added successfully.", provider.Id, provider.Name);
            return Result.Success();
        }
        catch (ValidationException ex) {
            var errors = string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"));
            Logger.LogWarning("Error adding the new provider. Validation errors:\n{Errors}", errors);
            Output.WriteLine($"[red]We found some problems while adding the provider. Please correct the following errors and try again:\n{errors}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error adding the new provider.", ct);

    private async Task SetUpAsync(ProviderEntity provider, CancellationToken ct)
        => provider.Name = await Input.BuildTextPrompt<string>("What is the name of the LLM provider?")
                                .AddValidation(name => ProviderEntity.ValidateName(name, handler))
                                .ShowAsync(ct);
}
