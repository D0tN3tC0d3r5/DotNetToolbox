namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IHasChildren : INode {
    ICollection<INode> Children { get; }
    IParameter[] Parameters { get; }
    IArgument[] Options { get; }
    ICommand[] Commands { get; }
}
