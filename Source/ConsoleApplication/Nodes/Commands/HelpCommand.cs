using DotNetToolbox.ConsoleApplication.Nodes;

namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class HelpCommand : Command<ExitCommand> {
    public HelpCommand(IExecutableNode application)
        : base(application, "Help") {
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var name = Parent is INamedNode named
                       ? named.Name
                       : Application.Title;
        Output.WriteLine($"{name} Commands:");
        foreach (var command in Application.Children) {
            Output.WriteLine($"  {command.Name}");
        }
        return Result.SuccessTask();
    }
}
