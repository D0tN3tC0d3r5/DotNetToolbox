namespace ConsoleApplication.Nodes.Executables;

public class CommandFactory {
    public static TCommand Create<TCommand>(IApplication application)
        where TCommand : Command<TCommand>
        => Create<TCommand>(application, default!, default!);

    public static TCommand Create<TCommand>(IApplication application, string name)
        where TCommand : Command<TCommand>
        => Create<TCommand>(application, default!, name);

    public static TCommand Create<TCommand>(IApplication application, ICommand owner)
        where TCommand : Command<TCommand>
        => CreateInstance.Of<TCommand>(application.ServiceProvider, application, owner, default!);

    public static TCommand Create<TCommand>(IApplication application, ICommand owner, string name)
        where TCommand : Command<TCommand>
        => CreateInstance.Of<TCommand>(application.ServiceProvider, application, owner, name);
}
