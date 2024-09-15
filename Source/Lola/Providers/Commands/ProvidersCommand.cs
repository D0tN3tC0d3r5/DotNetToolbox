namespace Lola.Providers.Commands;

public class ProvidersCommand(IHasChildren parent)
    : Command<ProvidersCommand>(parent, "Providers", n => {
        n.Description = "Manage LLM Providers.";
        n.Help = "Register, update, or remove LLM providers.";
        n.AddCommand<ProviderListCommand>();
        n.AddCommand<ProviderAddCommand>();
        n.AddCommand<ProviderViewCommand>();
        n.AddCommand<ProviderUpdateCommand>();
        n.AddCommand<ProviderRemoveCommand>();
        n.AddCommand<HelpCommand>();
        n.AddCommand<BackCommand>();
        n.AddCommand<ExitCommand>();
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Showing Providers main menu...");
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices(Commands.ToArray(c => c.Name))
                                .ShowAsync(lt);

        var command = Commands.FirstOrDefault(i => i.Name == choice);
        return command is null
            ? Result.Success()
            : await command.Execute([], lt);

        string MapTo(string item) => Commands.FirstOrDefault(i => i.Name == item)?.Description ?? string.Empty;
    }, "Error displaying provider's menu.", ct);
}
