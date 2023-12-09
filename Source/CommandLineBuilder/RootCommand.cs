namespace DotNetToolbox.CommandLineBuilder;

public interface IRootCommand {
    internal static string ExecutableName { get; } = GetFileNameWithoutExtension(GetCommandLineArgs()[0]);
}

public abstract class RootCommand<TCommand> : CommandBase<TCommand>, IRootCommand
    where TCommand : RootCommand<TCommand> {

    protected RootCommand(OutputWriter? writer = null)
        : base(TokenType.Root, IRootCommand.ExecutableName) {
        Writer = writer ?? new();
        Add(new VersionFlag());
        Add(new HelpFlag());
        Add(new VerboseLevelOption());
        Add(new NoColorFlag());
    }
}

public sealed class RootCommand(OutputWriter? writer = null) : RootCommand<RootCommand>(writer);
