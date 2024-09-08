namespace AI.Sample.Personas.Commands;

public class PersonasCommand : Command<PersonasCommand> {
    public PersonasCommand(IHasChildren parent)
        : base(parent, "Personas", []) {
        Description = "Manage AI Personas.";

        AddCommand<PersonaListCommand>();
        AddCommand<PersonaCreateCommand>();
        //AddCommand<PersonaUpdateCommand>();
        //AddCommand<PersonaRemoveCommand>();
        //AddCommand<PersonaViewCommand>();
        AddCommand<HelpCommand>();
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) {
        var choice = Input.BuildSelectionPrompt<string>("What would you like to do?")
                          .ConvertWith(MapTo)
                          .AddChoices("List",
                                      "Create",
                                      //"Info",
                                      //"Select",
                                      //"Update",
                                      //"Remove",
                                      "Help",
                                      "Back",
                                      "Exit").Show();

        var personaHandler = Application.Services.GetRequiredService<IPersonaHandler>();
        var command = choice switch {
            "List" => new PersonaListCommand(this, personaHandler),
            "Create" => new PersonaCreateCommand(this, personaHandler),
            //"Info" => new PersonaViewCommand(this, personaHandler, providerHandler),
            //"Select" => new PersonaSelectCommand(this, personaHandler),
            //"Update" => new PersonaUpdateCommand(this, personaHandler, providerHandler),
            //"Remove" => new PersonaRemoveCommand(this, personaHandler),
            "Help" => new HelpCommand(this),
            "Exit" => new ExitCommand(this),
            _ => (ICommand?)null,
        };
        return command?.Execute([], ct) ?? Result.SuccessTask();

        static string MapTo(string choice) => choice switch {
            "List" => "List Personas",
            "Create" => "Add a New Persona",
            //"Info" => "View the Details of a Persona",
            //"Select" => "Select the Default Persona",
            //"Update" => "Update a Persona",
            //"Remove" => "Delete a Persona",
            "Help" => "Help",
            "Back" => "Back",
            "Exit" => "Exit",
            _ => string.Empty,
        };
    }
}
