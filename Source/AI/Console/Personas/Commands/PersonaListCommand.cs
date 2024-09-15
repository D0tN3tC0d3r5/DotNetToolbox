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

        var sortedList = personas.OrderBy(p => p.Name);
        ShowList(sortedList);

        return Result.Success();
    }, "Error listing personas.");

    private void ShowList(IEnumerable<PersonaEntity> personas) {
        var sortedPersonas = personas.OrderBy(m => m.Name);
        var table = new Table();
        table.Expand();
        table.AddColumn(new("[yellow]Name[/]"));
        table.AddColumn(new("[yellow]Role[/]"));
        table.AddColumn(new("[yellow]Main Goal[/]"));
        foreach (var persona in sortedPersonas)
            table.AddRow(persona.Name, persona.Role, persona.Goals.FirstOrDefault() ?? "[red][Undefined][/]");
        Output.Write(table);
    }
}
