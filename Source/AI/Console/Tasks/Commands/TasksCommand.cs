namespace AI.Sample.Tasks.Commands;

public class TasksCommand : Command<TasksCommand> {
    public TasksCommand(IHasChildren parent)
        : base(parent, "Tasks", []) {
        Description = "Manage AI Tasks.";

        AddCommand<TaskListCommand>();
        //AddCommand<TaskCreateCommand>();
        //AddCommand<TaskUpdateCommand>();
        //AddCommand<TaskRemoveCommand>();
        //AddCommand<TaskViewCommand>();
        AddCommand<HelpCommand>();
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                          .ConvertWith(MapTo)
                          .AddChoices("List",
                                      //"Create",
                                      //"Info",
                                      //"Select",
                                      //"Update",
                                      //"Remove",
                                      "Help",
                                      "Back",
                                      "Exit")
                                        .ShowAsync(ct);

        var taskHandler = Application.Services.GetRequiredService<ITaskHandler>();
        var command = choice switch {
            "List" => new TaskListCommand(this, taskHandler),
            //"Create" => new TaskCreateCommand(this, taskHandler, aiService),
            //"Info" => new TaskViewCommand(this, taskHandler, providerHandler),
            //"Select" => new TaskSelectCommand(this, taskHandler),
            //"Update" => new TaskUpdateCommand(this, taskHandler, providerHandler),
            //"Remove" => new TaskRemoveCommand(this, taskHandler),
            "Help" => new HelpCommand(this),
            "Exit" => new ExitCommand(this),
            _ => (ICommand?)null,
        };
    return command is null
            ? Result.Success()
            : await command.Execute([], ct);

        static string MapTo(string choice) => choice switch {
            "List" => "List Tasks",
            "Create" => "Add a New Task",
            //"Info" => "View the Details of a Task",
            //"Select" => "Select the Default Task",
            //"Update" => "Update a Task",
            //"Remove" => "Delete a Task",
            "Help" => "Help",
            "Back" => "Back",
            "Exit" => "Exit",
            _ => string.Empty,
        };
    }, "Error displaying the tasks menu.", ct);
}
