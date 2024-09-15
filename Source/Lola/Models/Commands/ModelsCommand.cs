namespace Lola.Models.Commands;

public class ModelsCommand(IHasChildren parent)
    : Command<ModelsCommand>(parent, "Models", n => {
        n.Description = "Manage models";
        n.Help = "Register, update, or remove models from a specific LLM provider.";
        n.AddCommand<ModelListCommand>();
        n.AddCommand<ModelAddCommand>();
        n.AddCommand<ModelUpdateCommand>();
        n.AddCommand<ModelRemoveCommand>();
        n.AddCommand<ModelViewCommand>();
        n.AddCommand<HelpCommand>();
        n.AddCommand<BackCommand>();
        n.AddCommand<ExitCommand>();
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Showing Models main menu...");
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices(Commands.ToArray(c => c.Name))
                                .ShowAsync(lt);

        var command = Commands.FirstOrDefault(i => i.Name == choice);
        return command is null
            ? Result.Success()
            : await command.Execute([], lt);

        string MapTo(string item) => Commands.FirstOrDefault(i => i.Name == item)?.Description ?? string.Empty;
    }, "Error displaying model's menu.", ct);
}
