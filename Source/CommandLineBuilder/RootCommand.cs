using static System.Environment;
using static System.IO.Path;

namespace DotNetToolbox.CommandLineBuilder;

public sealed class RootCommand : RootCommand<RootCommand> {
    internal static string ExecutableName { get; } = GetFileNameWithoutExtension(GetCommandLineArgs()[0]);

    public RootCommand(OutputWriter? writer = null)
        : base(writer) {
    }
}

public abstract class RootCommand<TCommand> : CommandBase<TCommand>
    where TCommand : RootCommand<TCommand> {

    protected RootCommand(OutputWriter? writer = null)
        : base(RootCommand.ExecutableName) {
        Writer = writer ?? new();
        Add(new VersionFlag());
        Add(new HelpFlag());
        Add(new VerboseLevelOption());
        Add(new NoColorFlag());
    }
}
