using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Models.Commands;

public class ModelAddCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
    : Command<ModelAddCommand>(parent, "Add", c => {
        c.Aliases = ["new"];
        c.Description = "Add a new model";
        c.Help = "Register a new model from a specific LLM provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing Models->Add command...");
            var providers = providerHandler.List();
            if (providers.Length == 0) {
                Output.WriteLine("[yellow bold]No providers available. Please add a provider first.[/]");
                Logger.LogInformation("No providers available. Create model action cancelled.");
                return Result.Invalid("No providers available.");
            }

            var provider = await Input.BuildSelectionPrompt<ProviderEntity>("Select a provider:")
                                      .ConvertWith(p => $"{p.Id}: {p.Name}")
                                      .AddChoices(providers)
                                      .ShowAsync(ct);
            var model = new ModelEntity();
            await SetUpAsync(model, provider, lt);

            modelHandler.Add(model);
            Output.WriteLine($"[green]Settings '{model.Name}' added successfully.[/]");
            Logger.LogInformation("Settings '{ModelKey}:{ModelName}' added successfully.", model.Key, model.Name);
            return Result.Success();
        }
        catch (ValidationException ex) {
            var errors = string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"));
            Logger.LogWarning("Error adding the new model. Validation errors:\n{Errors}", errors);
            Output.WriteLine($"[red]We found some problems while adding the model. Please correct the following errors and try again:\n{errors}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error adding a new model.", ct);

    private async Task SetUpAsync(ModelEntity model, ProviderEntity provider, CancellationToken ct) {
        model.Key = await Input.BuildTextPrompt<string>("Enter the model identifier:")
                               .AddValidation(key => ModelEntity.ValidateNewKey(key, modelHandler))
                               .ShowAsync(ct);
        model.Name = await Input.BuildTextPrompt<string>("Enter the model name:")
                                .AddValidation(name => ModelEntity.ValidateNewName(name, modelHandler))
                                .ShowAsync(ct);
        model.ProviderId = provider.Id;
        model.MaximumContextSize = await Input.BuildTextPrompt<uint>("Enter the maximum context size:")
                                              .ShowAsync(ct);
        model.MaximumOutputTokens = await Input.BuildTextPrompt<uint>("Enter the maximum output tokens:")
                                               .ShowAsync(ct);
        model.InputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the input cost per million tokens:")
                                                     .AddValidation(ModelEntity.ValidateInputCost)
                                                     .ShowAsync(ct);
        model.OutputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the output cost per million tokens:")
                                                      .AddValidation(ModelEntity.ValidateOutputCost)
                                                      .ShowAsync(ct);
        model.TrainingDateCutOff = await Input.BuildTextPrompt<DateOnly?>("Enter the training data cut-off date (YYYY-MM-DD):")
                                              .AddValidation(ModelEntity.ValidateDateCutOff)
                                              .ShowAsync(ct);
    }
}
