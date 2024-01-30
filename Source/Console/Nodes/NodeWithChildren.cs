namespace DotNetToolbox.ConsoleApplication.Nodes;

public abstract class NodeWithChildren<TCommand>(IHasChildren parent, string name, params string[] aliases)
    : Node<TCommand>(parent, name, aliases), IHasChildren
    where TCommand : NodeWithChildren<TCommand> {
    public ICollection<INode> Children { get; } = [];

    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    public ICommand AddChildCommand(string name, Delegate action)
        => AddChildCommand(name, Array.Empty<string>(), action);
    public ICommand AddChildCommand(string name, string alias, Delegate action)
        => AddChildCommand(name, [alias], action);
    internal ICommand AddChildCommand(string name, string[] aliases, Delegate action) {
        var actionWrapper = ConvertToActionWrapper<Command>(action);
        var child = CreateInstance.Of<Command>(this, name, aliases, actionWrapper);
        Children.Add(child);
        return child;
    }

    public ICommand AddChildCommand<TChildCommand>()
        where TChildCommand : NodeWithChildren<TChildCommand>, ICommand {
        var child = CreateInstance.Of<TChildCommand>(Application.Services, this);
        Children.Add(child);
        return child;
    }

    public IFlag AddFlag(string name, Delegate? action = null) => AddFlag(name, Array.Empty<string>(), action);
    public IFlag AddFlag(string name, string alias, Delegate? action = null) => AddFlag(name, [alias], action);
    internal IFlag AddFlag(string name, string[] aliases, Delegate? action = null) {
        var actionWrapper = ConvertToActionWrapper<Flag>(action);
        var child = CreateInstance.Of<Flag>(this, name, aliases, actionWrapper);
        Children.Add(child);
        return child;
    }

    public IFlag AddFlag<TFlag>()
        where TFlag : Flag<TFlag>, IFlag {
        var child = CreateInstance.Of<TFlag>(Application.Services, this);
        Children.Add(child);
        return child;
    }

    public IOption AddOption(string name) => AddOption(name, Array.Empty<string>());
    public IOption AddOption(string name, string alias) => AddOption(name, [alias]);
    internal IOption AddOption(string name, string[] aliases) {
        var child = CreateInstance.Of<Option>(this, name, aliases);
        Children.Add(child);
        return child;
    }

    public IOption AddOption<TOption>()
        where TOption : Option<TOption>, IOption {
        var child = CreateInstance.Of<TOption>(Application.Services, this);
        Children.Add(child);
        return child;
    }

    public IParameter AddParameter(string name) => AddParameter(name, null);
    internal IParameter AddParameter(string name, string? defaultValue) {
        var child = CreateInstance.Of<Parameter>(this, name, defaultValue);
        Children.Add(child);
        return child;
    }

    public IParameter AddParameter<TParameter>()
        where TParameter : Parameter<TParameter>, IParameter {
        var child = CreateInstance.Of<TParameter>(Application.Services, this);
        Children.Add(child);
        return child;
    }

    private static Func<TNode, CancellationToken, Task<Result>> ConvertToActionWrapper<TNode>(Delegate? action)
        => action switch {
            null => (_, _) => SuccessTask(),
            Action func => (_, ct) => Task.Run(() => func(), ct).ContinueWith(_ => Success(), ct, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current),
            Action<TNode> func => (c, ct) => Task.Run(() => func(c), ct).ContinueWith(_ => Success(), ct, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current),
            Func<Result> func => (_, _) => Task.FromResult(func()),
            Func<Task<Result>> func => (_, _) => func(),
            Func<Task> func => (_, ct) => func().ContinueWith(_ => Success(), ct),
            Func<CancellationToken, Task<Result>> func => (_, ct) => func(ct),
            Func<CancellationToken, Task> func => (_, ct) => func(ct).ContinueWith(_ => Success(), ct),
            Func<TNode, Result> func => (cmd, _) => Task.FromResult(func(cmd)),
            Func<TNode, Task<Result>> func => (cmd, _) => func(cmd),
            Func<TNode, Task> func => (cmd, ct) => func(cmd).ContinueWith(_ => Success(), ct),
            Func<TNode, CancellationToken, Task<Result>> func => (cmd, ct) => func(cmd, ct),
            Func<TNode, CancellationToken, Task> func => (cmd, ct) => func(cmd, ct).ContinueWith(_ => Success(), ct),
            _ => throw new ArgumentException("Unsupported delegate type for action", nameof(action)),
        };
}
