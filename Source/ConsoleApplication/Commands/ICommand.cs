namespace DotNetToolbox.ConsoleApplication.Commands;
public interface ICommand<out TCommand>
    where TCommand : class, ICommand<TCommand> {
    IApplication Application { get; }

    string Name { get; }
    string Aliases { get; }
    string? Description { get; }
    string[] Arguments { get; }
    ILogger<TCommand> Logger { get; }

    Result Execute();
    Task<Result> ExecuteAsync();
}
