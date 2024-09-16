using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Providers.Commands;

public class ProviderUpdateCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderUpdateCommand>(parent, "Update", n => {
        n.Aliases = ["edit"];
        n.Description = "Update provider";
        n.Help = "Update a LLM provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing Providers->Update command...");
            var providers = handler.List();
            if (providers.Length == 0) {
                Output.WriteLine("[yellow]No providers found.[/]");
                Logger.LogInformation("No providers found. Remove provider action cancelled.");
                return Result.Success();
            }
            var provider = await this.SelectEntityAsync<ProviderEntity, uint>(providers.OrderBy(p => p.Name), m => m.Name, lt);
            if (provider is null) {
                Logger.LogInformation("Provider updated action cancelled.");
                return Result.Success();
            }
            await SetUpAsync(provider, lt);

            handler.Update(provider);
            Output.WriteLine($"[green]Provider '{provider.Name}' updated successfully.[/]");
            Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' updated successfully.", provider.Key, provider.Name);
            return Result.Success();
        }
        catch (ValidationException ex) {
            var errors = string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"));
            Logger.LogWarning("Error updating the provider. Validation errors:\n{Errors}", errors);
            Output.WriteLine($"[red]We found some problems while updating the provider. Please correct the following errors and try again:\n{errors}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error updating the provider.", ct);

    private async Task SetUpAsync(ProviderEntity provider, CancellationToken ct)
        => provider.Name = await Input.BuildTextPrompt<string>("Enter the new name for the provider")
                                .AddValidation(name => ProviderEntity.ValidateName(name, handler))
                                .ShowAsync(ct);
}
