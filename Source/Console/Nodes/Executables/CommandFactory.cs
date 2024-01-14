namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public static class CommandFactory {
    public static TCommand Create<TCommand>(IApplication application)
        where TCommand : CommandBase<TCommand>
        => Create<TCommand>(application, default!, default!);

    public static TCommand Create<TCommand>(IApplication application, string name)
        where TCommand : CommandBase<TCommand>
        => Create<TCommand>(application, default!, name);

    public static TCommand Create<TCommand>(IApplication application, ICommand owner)
        where TCommand : CommandBase<TCommand>
        => CreateInstance.Of<TCommand>(application.ServiceProvider, application, owner, default!);

    public static TCommand Create<TCommand>(IApplication application, ICommand owner, string name)
        where TCommand : CommandBase<TCommand>
        => CreateInstance.Of<TCommand>(application.ServiceProvider, application, owner, name);
}
