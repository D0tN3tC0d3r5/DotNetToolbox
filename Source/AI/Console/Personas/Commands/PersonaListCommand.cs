namespace AI.Sample.Personas.Commands;

public class PersonaListCommand : Command<PersonaListCommand> {
    private readonly IPersonaHandler _personaHandler;

    public PersonaListCommand(IHasChildren parent, IPersonaHandler personaHandler)
        : base(parent, "List", ["ls"]) {
        _personaHandler = personaHandler;
        Description = "List all personas or personas for a specific provider.";
    }

    protected override Result Execute() => this.HandleCommand(() => {
        var personas = _personaHandler.List();

        if (personas.Length == 0) {
            Output.WriteLine("[yellow]No personas found.[/]");
            Output.WriteLine();

            return Result.Success();
        }

        var sortedPersonas = personas.OrderBy(m => m.Name);

        var table = new Table();
        table.Expand();

        // Add columns
        table.AddColumn(new("[yellow]Name[/]"));
        table.AddColumn(new("[yellow]Role[/]"));
        table.AddColumn(new("[yellow]Main Goal[/]"));

        foreach (var persona in sortedPersonas) {
            table.AddRow(
                persona.Name,
                persona.Role,
                persona.Goals[0]
            );
        }

        Output.Write(table);
        Output.WriteLine();
        return Result.Success();
    }, "Error listing personas.");
}
