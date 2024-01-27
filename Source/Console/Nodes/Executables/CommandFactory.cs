namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public static class CommandFactory {
    public static TCommand Create<TCommand>(IApplication application)
        where TCommand : NodeWithArguments<TCommand>
        => Create<TCommand>(application, default!, default!);

    public static TCommand Create<TCommand>(IApplication application, string name)
        where TCommand : NodeWithArguments<TCommand>
        => Create<TCommand>(application, default!, name);

    public static TCommand Create<TCommand>(IApplication application, ICommand owner)
        where TCommand : NodeWithArguments<TCommand>
        => CreateInstance.Of<TCommand>(application.Services, application, owner, default!);

    public static TCommand Create<TCommand>(IApplication application, ICommand owner, string name)
        where TCommand : NodeWithArguments<TCommand>
        => CreateInstance.Of<TCommand>(application.Services, application, owner, name);
}
