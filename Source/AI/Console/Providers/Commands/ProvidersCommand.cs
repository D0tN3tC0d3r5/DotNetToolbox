namespace AI.Sample.Providers.Commands;

public class ProvidersCommand
    : Command<ProvidersCommand> {
    public ProvidersCommand(IHasChildren parent)
        : base(parent, "Providers", n => {
            n.Description = "Manage AI Providers.";
            n.Help = "Register, update, or remove AI providers to use with your AI agents.";
            n.AddCommand<ProviderListCommand>();
            n.AddCommand<ProviderAddCommand>();
            n.AddCommand<ProviderUpdateCommand>();
            n.AddCommand<ProviderRemoveCommand>();
            n.AddCommand<HelpCommand>();
            n.AddCommand<ExitCommand>();
        }) {
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Providers command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices(Commands.ToArray(c => c.Name))
                                .ShowAsync(cts.Token);

        var command = Commands.FirstOrDefault(i => i.Name == choice);
        return command is null
            ? Result.Success()
            : await command.Execute([], cts.Token);

        string MapTo(string choice) => Commands.FirstOrDefault(i => i.Name == choice)?.Description ?? string.Empty;
    }, "Error displaying provider's menu.", ct);
}
