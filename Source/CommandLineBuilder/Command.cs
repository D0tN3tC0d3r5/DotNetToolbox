namespace DotNetToolbox.CommandLineBuilder;

public sealed class Command : Command<Command> {
    public Command(string name, string? description = null)
        : base(name, description) {
    }
}

public abstract class Command<TCommand> : CommandBase<TCommand>
    where TCommand : Command<TCommand> {
    protected Command(string name, string? description = null)
        : base(name, description) {
        Add(new HelpFlag());
    }
}
