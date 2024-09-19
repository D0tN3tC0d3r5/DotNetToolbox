using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Models.Commands;

public class ModelUpdateCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
    : Command<ModelUpdateCommand>(parent, "Update", n => {
        n.Aliases = ["edit"];
        n.Description = "Update model";
        n.Help = "Update an existing model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing Models->Update command...");
            var models = modelHandler.List();
            if (models.Length == 0) {
                Output.WriteLine("[yellow]No models found.[/]");
                Logger.LogInformation("No models found. Remove model action cancelled.");
                return Result.Success();
            }
            var model = await this.SelectEntityAsync<ModelEntity, uint>(models.OrderBy(m => m.ProviderId).ThenBy(m => m.Name), m => m.Name, lt);
            if (model is null) {
                Logger.LogInformation("No model selected.");
                return Result.Success();
            }

            await SetUpAsync(model, lt);
            modelHandler.Update(model);
            Logger.LogInformation("Settings '{ModelKey}:{ModelName}' updated successfully.", model.Key, model.Name);
            Output.WriteLine("[green]Settings updated successfully.[/]");
            return Result.Success();
        }
        catch (ValidationException ex) {
            var errors = string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"));
            Logger.LogWarning("Error updating the model. Validation errors:\n{Errors}", errors);
            Output.WriteLine($"[red]We found some problems while updating the model. Please correct the following errors and try again:\n{errors}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error updating the model.", ct);

    private async Task SetUpAsync(ModelEntity model, CancellationToken ct) {
        var currentProvider = providerHandler.GetById(model.ProviderId);
        var provider = await Input.BuildSelectionPrompt<ProviderEntity>("Select a provider:")
                                  .ConvertWith(p => $"{p.Id}: {p.Name}")
                                  .WithDefault(currentProvider!)
                                  .AddChoices(providerHandler.List())
                                  .ShowAsync(ct);
        model.ProviderId = provider.Id;

        model.Key = await Input.BuildTextPrompt<string>("Enter the model identifier:")
                               .WithDefault(model.Key)
                               .AddValidation(key => ModelEntity.ValidateKey(key, modelHandler))
                               .ShowAsync(ct);
        model.Name = await Input.BuildTextPrompt<string>("Enter the model name:")
                                .WithDefault(model.Name)
                                .AddValidation(name => ModelEntity.ValidateName(name, modelHandler))
                                .ShowAsync(ct);
        model.MaximumContextSize = await Input.BuildTextPrompt<uint>("Enter the maximum context size:")
                                              .WithDefault(model.MaximumContextSize)
                                              .ShowAsync(ct);
        model.MaximumOutputTokens = await Input.BuildTextPrompt<uint>("Enter the maximum output tokens:")
                                               .WithDefault(model.MaximumOutputTokens)
                                               .ShowAsync(ct);
        model.InputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the input cost per million tokens:")
                                                     .WithDefault(model.InputCostPerMillionTokens)
                                                     .AddValidation(ModelEntity.ValidateInputCost)
                                                     .ShowAsync(ct);
        model.OutputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the output cost per million tokens:")
                                                      .WithDefault(model.OutputCostPerMillionTokens)
                                                      .AddValidation(ModelEntity.ValidateOutputCost)
                                                      .ShowAsync(ct);
        model.TrainingDateCutOff = await Input.BuildTextPrompt<DateOnly?>("Enter the training data cut-off date (YYYY-MM-DD):")
                                              .WithDefault(model.TrainingDateCutOff)
                                              .AddValidation(ModelEntity.ValidateDateCutOff)
                                              .ShowAsync(ct);
    }
}
