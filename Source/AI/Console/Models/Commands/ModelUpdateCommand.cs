namespace AI.Sample.Models.Commands;

public class ModelUpdateCommand : Command<ModelUpdateCommand> {
    private readonly IModelHandler _handler;
    private readonly IProviderHandler _providerHandler;

    public ModelUpdateCommand(IHasChildren parent, IModelHandler handler, IProviderHandler providerHandler)
        : base(parent, "Update", ["edit"]) {
        _handler = handler;
        _providerHandler = providerHandler;
        Description = "Update an existing model.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var model = this.EntitySelectionPrompt(_handler.List(), "show", "Model", m => m.Key, m => m.Name);
        if (model is null) {
            Logger.LogInformation("Model updated action cancelled.");
            return Result.SuccessTask();
        }

        model.Key = Input.TextPrompt("Enter the new identifier for the model")
                         .For("identifier").WithDefault(model.Key).AsRequired();

        model.Name = Input.TextPrompt("Enter the new name for the model")
                          .For("name").WithDefault(model.Name).AsRequired();

        var currentProvider = _providerHandler.GetByKey(model.ProviderKey);
        var provider = Input.SelectionPrompt<ProviderEntity>("Select a provider:")
                            .For("provider").ConvertWith(p => $"{p.Key}: {p.Name}")
                            .WithDefault(currentProvider).AddChoices(_providerHandler.List())
                            .Show();
        model.ProviderKey = provider.Key;

        model.MaximumContextSize = Input.TextPrompt<uint>("Enter the new maximum context size")
                                        .For("maximum context size").WithDefault(model.MaximumContextSize)
                                        .Validate(size => size > 0, "Maximum context size must be greater than 0.");

        model.MaximumOutputTokens = Input.TextPrompt<uint>("Enter the new maximum output tokens")
                                         .For("maximum output tokens").WithDefault(model.MaximumOutputTokens)
                                         .Validate(tokens => tokens > 0, "Maximum output tokens must be greater than 0.");

        model.InputCostPerMillionTokens = Input.TextPrompt<decimal>("Enter the new input cost per million tokens")
                                               .For("input cost per million tokens").WithDefault(model.InputCostPerMillionTokens)
                                               .Validate(cost => cost >= 0, "Cost must be non-negative.");

        model.OutputCostPerMillionTokens = Input.TextPrompt<decimal>("Enter the new output cost per million tokens")
                                                .For("output cost per million tokens").WithDefault(model.OutputCostPerMillionTokens)
                                                .Validate(cost => cost >= 0, "Cost must be non-negative.");

        model.TrainingDataCutOff = Input.TextPrompt<DateOnly>("Enter the new training cut-off date (YYYY-MM-DD)")
                                        .For("training cut-off date").WithDefault(model.TrainingDataCutOff)
                                        .Validate(date => date <= DateOnly.FromDateTime(DateTime.Now), "Cut-off date cannot be in the future.");

        try {
            _handler.Update(model);
            Logger.LogInformation("Model '{ModelKey}:{ModelName}' updated successfuly.", model.Key, model.Name);
            Output.WriteLine("[green]Model updated successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Error updating the model '{ModelKey}:{ModelName}'.", model.Key, model.Name);
            Output.WriteError("Error updating the model.");
            return Result.ErrorTask(ex.Message);
        }
        //model.Key = AnsiConsole.TextPrompt("[yellow]Enter the new identifier for the model[/]", model.Key);
        //model.Name = AnsiConsole.TextPrompt("[yellow]Enter the new name for the model[/]", model.Name);

        //var providers = _providerHandler.List();
        //var currentProvider = _providerHandler.GetByKey(model.ProviderKey);
        //var selectTitle = $"[yellow]SelectionPrompt a new provider[/] [green]({IsNotNull(currentProvider).Name})[/]";
        //var providerPrompt = new SelectionPrompt<ProviderEntity>()
        //    .Title(selectTitle)
        //    .AddChoices(providers)
        //    .UseConverter(p => $"{p.Key}: {p.Name}");
        //var selectedProvider = AnsiConsole.Show(providerPrompt);
        //model.ProviderKey = selectedProvider.Key;
        //Output.WriteLine($"{selectTitle}: {selectedProvider.Name}");

        //model.MaximumContextSize = AnsiConsole.TextPrompt("[yellow]Enter the new maximum context size[/]", model.MaximumContextSize);
        //model.MaximumOutputTokens = AnsiConsole.TextPrompt("[yellow]Enter the new maximum output tokens[/]", model.MaximumOutputTokens);
        //model.InputCostPerMillionTokens = AnsiConsole.TextPrompt<decimal>("[yellow]Enter the input cost per million tokens[/]", model.InputCostPerMillionTokens);
        //model.OutputCostPerMillionTokens = AnsiConsole.TextPrompt<decimal>("[yellow]Enter the output cost per million tokens[/]", model.OutputCostPerMillionTokens);
        //model.TrainingDataCutOff = AnsiConsole.TextPrompt<DateOnly>("[yellow]Enter the training data cut-off date[/]", model.TrainingDataCutOff);

        //try {
        //    _handler.Update(model);
        //    Output.WriteLine($"[green]Model '{model.Name}' updated successfully.[/]");
        //    Logger.LogInformation("Model '{ModelKey}:{ModelName}' updated successfuly.", model.Key, model.Name);
        //    return Result.SuccessTask();
        //}
        //catch (Exception ex) {
        //    Output.WriteError("Error updating the model.");
        //    Logger.LogError(ex, "Error updating the model '{ModelKey}:{ModelName}'.", model.Key, model.Name);
        //    return Result.ErrorTask(ex.Message);
        //}
    }
}
