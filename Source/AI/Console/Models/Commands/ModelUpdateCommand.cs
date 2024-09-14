using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Models.Commands;

public class ModelUpdateCommand(IHasChildren parent, IModelHandler handler, IProviderHandler providerHandler)
    : Command<ModelUpdateCommand>(parent, "Update", n => {
        n.Aliases = ["edit"];
        n.Description = "Update model";
        n.Help = "Update an existing model.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Update command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var model = await this.SelectEntityAsync(handler.List(), "show", "Settings", m => m.Key, m => m.Name, cts.Token);
        if (model is null) {
            Logger.LogInformation("No model selected.");
            Output.WriteLine();
            return Result.Success();
        }

        await SetUpAsync(model, cts.Token);

        handler.Update(model);
        Logger.LogInformation("Settings '{ModelKey}:{ModelName}' updated successfully.", model.Key, model.Name);
        Output.WriteLine("[green]Settings updated successfully.[/]");
        Output.WriteLine();

        return Result.Success();
    }, "Error updating the model.", ct);

    private async Task SetUpAsync(ModelEntity model, CancellationToken ct) {
        model.Key = await Input.BuildTextPrompt<string>("Enter the new identifier for the model")
                               .For("identifier").WithDefault(model.Key)
                               .AsRequired()
                               .ShowAsync(ct);

        model.Name = await Input.BuildTextPrompt<string>("Enter the new name for the model")
                                .For("name").WithDefault(model.Name)
                                .AsRequired()
                                .ShowAsync(ct);

        var currentProvider = providerHandler.GetByKey(model.ProviderKey);
        var provider = await Input.BuildSelectionPrompt<ProviderEntity>("Select a provider:")
                                  .ConvertWith(p => $"{p.Key}: {p.Name}")
                                  .WithDefault(currentProvider!)
                                  .AddChoices(providerHandler.List())
                                  .ShowAsync(ct);
        model.ProviderKey = provider.Key;

        model.MaximumContextSize = await Input.BuildTextPrompt<uint>("Enter the new maximum context size")
                                              .For("maximum context size")
                                              .WithDefault(model.MaximumContextSize)
                                              .Validate(size => size > 0, "Maximum context size must be greater than 0.")
                                              .ShowAsync(ct);

        model.MaximumOutputTokens = await Input.BuildTextPrompt<uint>("Enter the new maximum output tokens")
                                               .For("maximum output tokens")
                                               .WithDefault(model.MaximumOutputTokens)
                                               .Validate(tokens => tokens > 0, "Maximum output tokens must be greater than 0.")
                                               .ShowAsync(ct);

        model.InputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the new input cost per million tokens")
                                                     .For("input cost per million tokens")
                                                     .WithDefault(model.InputCostPerMillionTokens)
                                                     .Validate(cost => cost >= 0, "Cost must be non-negative.")
                                                     .ShowAsync(ct);

        model.OutputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the new output cost per million tokens")
                                                      .For("output cost per million tokens")
                                                      .WithDefault(model.OutputCostPerMillionTokens)
                                                      .Validate(cost => cost >= 0, "Cost must be non-negative.")
                                                      .ShowAsync(ct);

        model.TrainingDataCutOff = await Input.BuildTextPrompt<DateOnly>("Enter the new training cut-off date (YYYY-MM-DD)")
                                              .For("training cut-off date")
                                              .WithDefault(model.TrainingDataCutOff)
                                              .Validate(date => date <= DateOnly.FromDateTime(DateTime.Now), "Cut-off date cannot be in the future.")
                                              .ShowAsync(ct);
    }
}
