namespace AI.Sample.Tasks.Commands;

public class TaskListCommand(IHasChildren parent, ITaskHandler taskHandler)
    : Command<TaskListCommand>(parent, "List", n => {
        n.Aliases = ["ls"];
        n.Description = "List the existing tasks.";
    }) {
    protected override Result Execute() => this.HandleCommand(() => {
        Logger.LogInformation("Executing Tasks->List command...");
        var tasks = taskHandler.List();

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
    }, "Error listing the tasks.");
}
