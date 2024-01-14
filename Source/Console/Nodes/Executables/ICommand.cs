namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public interface ICommand : IArgument, IHasChildren, IExecutable;

public interface IAsyncCommand : IArgument, IHasChildren, IExecutable;
