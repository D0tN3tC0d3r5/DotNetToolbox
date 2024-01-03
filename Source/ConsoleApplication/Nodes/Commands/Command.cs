using DotNetToolbox.ConsoleApplication.Nodes;
using DotNetToolbox.ConsoleApplication.Nodes.Application;
using DotNetToolbox.ConsoleApplication.Nodes.Arguments;

namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

public abstract class Command<TCommand>
    : ICommand
    where TCommand : Command<TCommand> {

    protected Command(IExecutableNode parent, string? name = null) {
        Parent = parent;
        Application = FindApplication(parent);
        Name = GetName(name);
        Logger = Application.ServiceProvider.GetRequiredService<ILogger<TCommand>>();
        Output = Application.ServiceProvider.GetRequiredService<Output>();
        Input = Application.ServiceProvider.GetRequiredService<Input>();
        DateTime = Application.ServiceProvider.GetRequiredService<DateTimeProvider>();
        Guid = Application.ServiceProvider.GetRequiredService<GuidProvider>();
        FileSystem = Application.ServiceProvider.GetRequiredService<FileSystem>();
    }

    private static string GetName(string? name) {
        var typeName = typeof(TCommand).Name;
        return name ?? (typeName.EndsWith("Command") ? typeof(TCommand).Name[..7] : typeof(TCommand).Name);
    }

    private static IApplication FindApplication(IExecutableNode node) {
        while (node.Parent is not null) node = node.Parent;
        return (IApplication)node;
    }

    public ILogger<TCommand> Logger { get; }
    public IApplication Application { get; set; }
    public IExecutableNode? Parent { get; }
    public ICollection<INamedNode> Children { get; } = [];
    public string Name { get; set; }
    public string? Alias { get; init; }
    public string? Description { get; init; }
    public string[] Ids => Alias is null ? [Name] : [Name, Alias];

    public Output Output { get; init; }
    public Input Input { get; init; }
    public DateTimeProvider DateTime { get; init; }
    public GuidProvider Guid { get; init; }
    public FileSystem FileSystem { get; init; }

    public async Task<Result> ExecuteAsync(string[] input, CancellationToken ct) {
        var result = await ReadInput(input, ct);
        if (!result.IsSuccess) return result;
        return await ExecuteAsync(ct);
    }

    private async Task<Result> ReadInput(string[] input, CancellationToken ct) {
        for (var index = 0; index < input.Length; index++) {
            var id = input[index].TrimStart('-');
            var argument = Children.FirstOrDefault(arg => arg.Ids.Contains(id));
            switch (argument) {
                case IHasValue hasValue:
                    if (argument is IOption) index++;
                    if (index >= input.Length) return Result.Error($"Missing value for option '{id}'");
                    var argumentResult = await hasValue.SetValue(input[index], ct);
                    if (!argumentResult.IsSuccess) return argumentResult;
                    break;
                case ICommand command:
                    index++;
                    var commandResult = await command.ExecuteAsync(index >= input.Length ? [] : input[index..], ct);
                    if (!commandResult.IsSuccess) return commandResult;
                    break;
                default:
                    return await ProcessParameters(input, ct);
            }
        }
        return Result.Success();
    }

    private async Task<Result> ProcessParameters(string[] input, CancellationToken ct) {
        var parameters = Children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var index = 0;
        foreach (var parameter in parameters) {
            if (index >= input.Length && parameter.IsRequired)
                return Result.Error($"Missing value for parameter {index + 1}:'{parameter.Name}'");
            if (index >= input.Length) break;
            var result = await parameter.SetValue(input[index], ct);
            if (!result.IsSuccess) return result;
            index++;
        }

        return Result.Success();
    }

    protected abstract Task<Result> ExecuteAsync(CancellationToken ct);
}
