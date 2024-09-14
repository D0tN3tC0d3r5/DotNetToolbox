using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace AI.Sample.Providers.Commands;

public class ProviderAddCommand(IHasChildren parent, IProviderHandler handler)
    : Command<ProviderAddCommand>(parent, "Add", n => {
        n.Aliases = ["new"];
        n.Description = "Add provider";
        n.Help = "Register a new LLM provider to use with your AI agents.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing Providers->Add command...");
            var provider = await handler.Create(SetUpAsync, lt);
            handler.Add(provider);
            Output.WriteLine($"[green]Provider '{provider.Name}' added successfully.[/]");
            Logger.LogInformation("Provider '{ProviderKey}:{ProviderName}' added successfully.", provider.Key, provider.Name);
            Output.WriteLine();
            return Result.Success();
        } catch (ValidationException ex) {
            Logger.LogWarning("Error adding the new provider. Validation errors:\n{Errors}", string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}")));
            Output.WriteLine($"[red]The provider is invalid. Please correct the following errors and try again:\n{string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"))}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error adding the new provider.", ct);

    private async Task SetUpAsync(ProviderEntity provider, CancellationToken ct)
        => provider.Name = await Input.BuildTextPrompt<string>("What is the name of the LLM provider?")
                                .Validate(name => ProviderEntity.ValidateName(name, handler))
                                .ShowAsync(ct);
}
