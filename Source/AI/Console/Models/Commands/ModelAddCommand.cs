using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Models.Commands;

public class ModelAddCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
    : Command<ModelAddCommand>(parent, "Create", c => {
        c.Aliases = ["add", "new"];
        c.Description = "Add a new model.";
        c.Help = "Register a new model from a specific LLM provider.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Add command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var providers = providerHandler.List();
        if (providers.Length == 0) {
            Output.WriteLine("[yellow bold]No providers available. Please add a provider first.[/]");
            Logger.LogInformation("No providers available. Create model action cancelled.");
            return Result.Invalid("No providers available.");
        }

        var provider = await Input.BuildSelectionPrompt<ProviderEntity>("Select a provider:")
                                  .ConvertWith(p => $"{p.Key}: {p.Name}")
                                  .AddChoices(providers)
                                  .ShowAsync(ct);
        var model = modelHandler.Create(async m => await SetUpAsync(m, provider, cts.Token));

        modelHandler.Register(model);
        Output.WriteLine($"[green]Settings '{model.Name}' added successfully.[/]");
        Logger.LogInformation("Settings '{ModelKey}:{ModelName}' added successfully.", model.Key, model.Name);
        Output.WriteLine();

        return Result.Success();
    }, "Error adding a new model.", ct);

    private async Task SetUpAsync(ModelEntity model, ProviderEntity provider, CancellationToken ct) {
        model.Key = await Input.BuildTextPrompt<string>("Enter the model identifier:")
                         .For("Identifier")
                         .AsRequired()
                         .ShowAsync(ct);
        model.Name = await Input.BuildTextPrompt<string>("Enter the model name:")
                          .For("name")
                          .AsRequired()
                          .ShowAsync(ct);
        model.ProviderKey = provider.Key;
        model.MaximumContextSize = await Input.BuildTextPrompt<uint>("Enter the maximum context size:")
                                        .For("maximum context size")
                                        .AsRequired()
                                        .Validate(size => size > 0, "Maximum context size must be greater than 0.")
                                        .ShowAsync(ct);

        model.MaximumOutputTokens = await Input.BuildTextPrompt<uint>("Enter the maximum output tokens:")
                                         .For("maximum output tokens")
                                         .AsRequired()
                                         .Validate(tokens => tokens > 0, "Maximum output tokens must be greater than 0.")
                                         .ShowAsync(ct);
        model.InputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the input cost per million tokens:")
                                               .For("input cost per million tokens")
                                               .Validate(cost => cost >= 0, "Cost must be non-negative.")
                                               .ShowAsync(ct);
        model.OutputCostPerMillionTokens = await Input.BuildTextPrompt<decimal>("Enter the output cost per million tokens:")
                                                .For("output cost per million tokens")
                                                .Validate(cost => cost >= 0, "Cost must be non-negative.")
                                                .ShowAsync(ct);

        model.TrainingDataCutOff = await Input.BuildTextPrompt<DateOnly>("Enter the training data cut-off date (YYYY-MM-DD):")
                                        .For("training data cut-off date")
                                        .Validate(date => date <= DateOnly.FromDateTime(DateTime.Now), "Cut-off date cannot be in the future.")
                                        .ShowAsync(ct);
    }
}
