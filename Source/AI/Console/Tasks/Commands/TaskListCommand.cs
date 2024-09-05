namespace AI.Sample.Tasks.Commands;

public class TaskListCommand : Command<TaskListCommand> {
    private readonly ITaskHandler _taskHandler;

    public TaskListCommand(IHasChildren parent, ITaskHandler taskHandler)
        : base(parent, "List", ["ls"]) {
        _taskHandler = taskHandler;
        Description = "List all tasks or tasks for a specific provider.";
    }

    protected override Result Execute() {
        var tasks = _taskHandler.List();

        if (tasks.Length == 0) {
            Output.WriteLine("[yellow]No tasks found.[/]");
            Output.WriteLine();

            return Result.Success();
        }

        var sortedTasks = tasks.OrderBy(m => m.Name);

        var table = new Table();
        table.Expand();

        // Add columns
        table.AddColumn(new("[yellow]Name[/]"));
        table.AddColumn(new("[yellow]Main Goal[/]"));

        foreach (var task in sortedTasks) {
            table.AddRow(
                task.Name,
                task.Goals[0]
            );
        }

        Output.Write(table);
        Output.WriteLine();
        return Result.Success();
    }
}
