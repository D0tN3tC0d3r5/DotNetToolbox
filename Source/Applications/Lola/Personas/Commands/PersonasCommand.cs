namespace Lola.Personas.Commands;

public class PersonasCommand(IHasChildren parent)
    : Command<PersonasCommand>(parent, "Personas", n => {
        n.Description = "Manage Agent's Personas";
        n.AddCommand<PersonaListCommand>();
        n.AddCommand<PersonaGenerateCommand>();
        n.AddCommand<PersonaViewCommand>();
        n.AddCommand<PersonaUpdateCommand>();
        n.AddCommand<PersonaRemoveCommand>();
        n.AddCommand<HelpCommand>();
        n.AddCommand<BackCommand>();
        n.AddCommand<ExitCommand>();
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Personas->Main command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices(Commands.AsIndexed().OrderBy(i => i.Index).ToArray(c => c.Value.Name))
                                .ShowAsync(cts.Token);

        var command = Commands.FirstOrDefault(i => i.Name == choice);
        return command is null
                   ? Result.Success()
                   : await command.Execute([], cts.Token);

        string MapTo(string item) => Commands.FirstOrDefault(i => i.Name == item)?.Description ?? string.Empty;
    }, "Error displaying the persona menu.", ct);
}
