﻿namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

public abstract class Executable<TExecutable>
    : Node<TExecutable>, IExecutable
    where TExecutable : Executable<TExecutable> {

    protected Executable(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
        Output = Application.ServiceProvider.GetRequiredService<Output>();
        Input = Application.ServiceProvider.GetRequiredService<Input>();
        DateTime = Application.ServiceProvider.GetRequiredService<DateTimeProvider>();
        Guid = Application.ServiceProvider.GetRequiredService<GuidProvider>();
        FileSystem = Application.ServiceProvider.GetRequiredService<FileSystem>();
    }

    public ICollection<INode> Children { get; } = [];

    public Output Output { get; init; }
    public Input Input { get; init; }
    public DateTimeProvider DateTime { get; init; }
    public GuidProvider Guid { get; init; }
    public FileSystem FileSystem { get; init; }

    public async Task<Result> ExecuteAsync(string[] input, CancellationToken ct) {
        var result = await ReadInput(input, ct);
        return result.IsSuccess
                   ? await ExecuteAsync(ct)
                   : result;
    }

    protected abstract Task<Result> ExecuteAsync(CancellationToken ct);

    protected abstract Task<Result> ReadInput(string[] input, CancellationToken ct);
}
