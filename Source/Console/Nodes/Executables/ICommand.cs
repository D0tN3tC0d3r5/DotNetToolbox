namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public interface IAsyncCommand : IHasParent, IHasChildren, IExecutable;

public interface ICommand : IHasParent, IHasChildren, IExecutable;
