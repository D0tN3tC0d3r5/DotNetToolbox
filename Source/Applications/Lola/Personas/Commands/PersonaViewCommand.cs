namespace Lola.Personas.Commands;

public class PersonaViewCommand(IHasChildren parent, IPersonaHandler handler)
    : Command<PersonaViewCommand>(parent, "Info", n => {
        n.Aliases = ["i"];
        n.Description = "View persona details";
        n.Help = "Display detailed information about an agent's persona.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Personas->Info command...");
        var personas = handler.List();
        if (personas.Length == 0) {
            Output.WriteLine("[yellow]No personas found.[/]");
            Logger.LogInformation("No personas found. View persona action cancelled.");
            return Result.Success();
        }
        var persona = await this.SelectEntityAsync<PersonaEntity, uint>(personas.OrderBy(p => p.Name), p => p.Name, lt);
        if (persona is null) {
            Logger.LogInformation("No persona selected.");
            return Result.Success();
        }

        ShowDetails(persona);
        return Result.Success();
    }, "Error displaying the persona information.", ct);

    private void ShowDetails(PersonaEntity persona) {
        Output.WriteLine("[yellow]Persona Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {persona.Name}");
        Output.WriteLine($"[blue]Role:[/] {persona.Role}");
        Output.WriteLine("[blue]Goals:[/]");
        foreach (var goal in persona.Goals) {
            Output.WriteLine($" - {goal}");
        }
        Output.WriteLine("[blue]Expertise:[/]");
        Output.WriteLine(persona.Expertise);
        Output.WriteLine("[blue]Traits:[/]");
        foreach (var trait in persona.Traits) {
            Output.WriteLine($" - {trait}");
        }
        Output.WriteLine("[blue]Important:[/]");
        foreach (var important in persona.Important) {
            Output.WriteLine($" - {important}");
        }
        Output.WriteLine("[blue]Negative:[/]");
        foreach (var negative in persona.Negative) {
            Output.WriteLine($" - {negative}");
        }
        Output.WriteLine("[blue]Other:[/]");
        foreach (var other in persona.Other) {
            Output.WriteLine($" - {other}");
        }
        Output.WriteLine();
    }
}
