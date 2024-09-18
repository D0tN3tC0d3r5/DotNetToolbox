namespace Lola.Tasks.Commands;

public class TasksCommand(IHasChildren parent)
    : Command<TasksCommand>(parent, "Tasks", n => {
        n.Description = "Manage Tasks.";
        n.AddCommand<TaskListCommand>();
        //n.AddCommand<TaskCreateCommand>();
        //n.AddCommand<TaskUpdateCommand>();
        //n.AddCommand<TaskRemoveCommand>();
        //AddCommand<TaskViewCommand>();
        n.AddCommand<HelpCommand>();
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Tasks command...");
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices(Commands.ToArray(c => c.Name))
                                .ShowAsync(lt);

        var command = Commands.FirstOrDefault(i => i.Name == choice);
        return command is null
            ? Result.Success()
            : await command.Execute([], lt);

        string MapTo(string item) => Commands.FirstOrDefault(i => i.Name == item)?.Description ?? string.Empty;
    }, "Error displaying the tasks menu.", ct);
}
