namespace AI.Sample.Models.Commands;

public class ModelsCommand : Command<ModelsCommand> {
    public ModelsCommand(IHasChildren parent) : base(parent, "Models", []) {
        Description = "Manage AI Models.";

        AddCommand<ModelListCommand>();
        AddCommand<ModelAddCommand>();
        AddCommand<ModelUpdateCommand>();
        AddCommand<ModelRemoveCommand>();
        AddCommand<ModelViewCommand>();
        AddCommand<HelpCommand>();
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) {
        var choice = Input.BuildSelectionPrompt<string>("What would you like to do?")
                          .ConvertWith(MapTo)
                          .AddChoices("List",
                                      "Create",
                                      "Info",
                                      "Select",
                                      "Update",
                                      "Remove",
                                      "Help",
                                      "Back",
                                      "Exit").Show();

        var providerHandler = Application.Services.GetRequiredService<IProviderHandler>();
        var modelHandler = Application.Services.GetRequiredService<IModelHandler>();
        var command = choice switch {
            "List" => new ModelListCommand(this, modelHandler, providerHandler),
            "Create" => new ModelAddCommand(this, modelHandler, providerHandler),
            "Info" => new ModelViewCommand(this, modelHandler, providerHandler),
            "Select" => new ModelSelectCommand(this, modelHandler),
            "Update" => new ModelUpdateCommand(this, modelHandler, providerHandler),
            "Remove" => new ModelRemoveCommand(this, modelHandler),
            "Help" => new HelpCommand(this),
            "Exit" => new ExitCommand(this),
            _ => (ICommand?)null,
        };
        return command?.Execute([], ct) ?? Result.SuccessTask();

        static string MapTo(string choice) => choice switch {
            "List" => "List Models",
            "Create" => "Add a New Model",
            "Info" => "View the Details of a Model",
            "Select" => "Select the Default Model",
            "Update" => "Update a Model",
            "Remove" => "Delete a Model",
            "Help" => "Help",
            "Back" => "Back",
            "Exit" => "Exit",
            _ => string.Empty,
        };
    }
}
