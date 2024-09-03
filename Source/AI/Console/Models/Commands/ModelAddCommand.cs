namespace AI.Sample.Models.Commands;

public class ModelAddCommand : Command<ModelAddCommand> {
    private readonly IModelHandler _modelHandler;
    private readonly IProviderHandler _providerHandler;

    public ModelAddCommand(IHasChildren parent, IModelHandler modelHandler, IProviderHandler providerHandler)
        : base(parent, "Create", ["add", "new"]) {
        _modelHandler = modelHandler;
        _providerHandler = providerHandler;
        Description = "Create a new model.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        try {
            var providers = _providerHandler.List();
            if (providers.Length == 0) {
                Output.WriteLine("[yellow bold]No providers available. Please add a provider first.[/]");
                Logger.LogInformation("No providers available. Create model action cancelled.");
                return Result.InvalidTask("No providers available.");
            }

            var provider = Input.SelectionPrompt<ProviderEntity>("Select a provider:")
                                .ConvertWith(p => $"{p.Key}: {p.Name}")
                                .AddChoices(providers)
                                .Show();
            var model = _modelHandler.Create(m => SetUp(m, provider));

            _modelHandler.Register(model);
            Output.WriteLine($"[green]Settings '{model.Name}' added successfully.[/]");
            Logger.LogInformation("Settings '{ModelKey}:{ModelName}' added successfully.", model.Key, model.Name);
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error adding the new model.");
            Logger.LogError(ex, "Error adding the new model.");
            return Result.ErrorTask(ex.Message);
        }
    }

    private void SetUp(ModelEntity model, ProviderEntity provider) {
        model.Key = Input.TextPrompt("Enter the model identifier:")
                         .For("Identifier").AsRequired();
        model.Name = Input.TextPrompt("Enter the model name:")
                          .For("name").AsRequired();
        model.ProviderKey = provider.Key;
        model.MaximumContextSize = Input.TextPrompt<uint>("Enter the maximum context size:")
                                        .For("maximum context size").AsRequired()
                                        .Validate(size => size > 0, "Maximum context size must be greater than 0.");
        model.MaximumOutputTokens = Input.TextPrompt<uint>("Enter the maximum output tokens:")
                                         .For("maximum output tokens").AsRequired()
                                         .Validate(tokens => tokens > 0, "Maximum output tokens must be greater than 0.");
        model.InputCostPerMillionTokens = Input.TextPrompt<decimal>("Enter the input cost per million tokens:")
                                               .For("input cost per million tokens")
                                               .Validate(cost => cost >= 0, "Cost must be non-negative.");
        model.OutputCostPerMillionTokens = Input.TextPrompt<decimal>("Enter the output cost per million tokens:")
                                                .For("output cost per million tokens")
                                                .Validate(cost => cost >= 0, "Cost must be non-negative.");
        model.TrainingDataCutOff = Input.TextPrompt<DateOnly>("Enter the training data cut-off date (YYYY-MM-DD):")
                                        .For("training data cut-off date")
                                        .Validate(date => date <= DateOnly.FromDateTime(DateTime.Now), "Cut-off date cannot be in the future.");
    }
}
