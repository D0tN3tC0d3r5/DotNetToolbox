namespace AI.Sample.Models.Commands;

public class ModelsCommand(IHasChildren parent)
    : Command<ModelsCommand>(parent, "Models", n => {
        n.Description = "Manage AI Models.";
        n.Help = "Register, update, or remove models from a specific LLM provider.";
        n.AddCommand<ModelListCommand>();
        n.AddCommand<ModelAddCommand>();
        n.AddCommand<ModelUpdateCommand>();
        n.AddCommand<ModelRemoveCommand>();
        n.AddCommand<ModelViewCommand>();
        n.AddCommand<HelpCommand>();
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Models->Main command...");
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
    }, "Error displaying model's menu.", ct);
}
