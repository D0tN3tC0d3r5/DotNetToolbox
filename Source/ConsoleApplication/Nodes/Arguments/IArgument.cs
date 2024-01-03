using DotNetToolbox.ConsoleApplication.Nodes;

namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IArgument : INamedNode {
    ArgumentType ArgumentType { get; }
}
