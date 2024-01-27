//namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

//public sealed class AsyncCommand : AsyncCommand<AsyncCommand> {
//    internal AsyncCommand(IHasChildren node, string name, string[] aliases, Func<AsyncCommand, CancellationToken, Task<Result>> execute)
//        : base(node, name, aliases, execute) {
//    }
//}

//public class AsyncCommand<TCommand>
//    : NodeWithArguments<TCommand>
//    , IAsyncCommand
//    where TCommand : AsyncCommand<TCommand> {
//    private readonly Func<TCommand, CancellationToken, Task<Result>>? _execute;

//    internal AsyncCommand(IHasChildren node, string name, params string[] aliases)
//        : base(node, name, aliases) {
//    }

//    internal AsyncCommand(IHasChildren node, string name, string[] aliases, Func<TCommand, CancellationToken, Task<Result>> execute)
//        : this(node, name, aliases) {
//        _execute = IsNotNull(execute);
//    }

//    public sealed override async Task<Result> ExecuteAsync(string[] args, CancellationToken ct = default) {
//        var result = await ArgumentsReader.Read(args, Children.ToArray(), ct);
//        return result.IsSuccess
//                   ? await (_execute?.Invoke((TCommand)this, ct) ?? ExecuteAsync(ct))
//                   : result;
//    }

//    protected virtual Task<Result> ExecuteAsync(CancellationToken ct) => SuccessTask();
//}
