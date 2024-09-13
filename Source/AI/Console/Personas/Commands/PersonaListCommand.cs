namespace AI.Sample.Personas.Commands;

public class PersonaListCommand(IHasChildren parent, IPersonaHandler personaHandler)
    : Command<PersonaListCommand>(parent, "List", n => {
        n.Aliases = ["ls"];
        n.Description = "List personas.";
        n.Help = "List all the agent's personas.";
    }) {
    protected override Result Execute() => this.HandleCommand(() => {
        Logger.LogInformation("Executing Personas->List command...");
        var personas = personaHandler.List();

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
