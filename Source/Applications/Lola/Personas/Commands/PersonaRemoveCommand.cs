namespace Lola.Personas.Commands;

public class PersonaRemoveCommand(IHasChildren parent, IPersonaHandler handler)
    : Command<PersonaRemoveCommand>(parent, "Remove", n => {
        n.Aliases = ["delete", "del"];
        n.Description = "Remove a persona";
        n.Help = "Remove an existing agent's persona.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Personas->Remove command...");
        var personas = handler.List();
        if (personas.Length == 0) {
            Output.WriteLine("[yellow]No personas found.[/]");
            Logger.LogInformation("No personas found. Remove persona action cancelled.");
            return Result.Success();
        }
        var persona = await this.SelectEntityAsync<PersonaEntity, uint>(personas.OrderBy(p => p.Name), p => p.Name, lt);
        if (persona is null) {
            Logger.LogInformation("No persona selected.");
            return Result.Success();
        }

        if (!await Input.ConfirmAsync($"Are you sure you want to remove the persona '{persona.Name}' ({persona.Id})?", lt)) {
            Logger.LogInformation("Persona removal cancelled by user.");
            return Result.Invalid("Action cancelled.");
        }

        handler.Remove(persona.Id);
        Output.WriteLine($"[green]Persona '{persona.Name}' removed successfully.[/]");
        Logger.LogInformation("Persona '{PersonaId}:{PersonaName}' removed successfully.", persona.Id, persona.Name);
        return Result.Success();
    }, "Error removing the persona.", ct);
}
