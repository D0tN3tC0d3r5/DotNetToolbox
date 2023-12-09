namespace DotNetToolbox.CommandLineBuilder;

public interface ICommand {
}

public abstract class Command<TCommand> : CommandBase<TCommand>, ICommand
    where TCommand : Command<TCommand> {
    protected Command(string name, string? description = null)
        : base(TokenType.Command, name, description) {
        Add(new HelpFlag());
    }
}

public sealed class Command(string name, string? description = null) : Command<Command>(name, description);
