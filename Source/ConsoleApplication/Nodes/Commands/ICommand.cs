using DotNetToolbox.ConsoleApplication.Nodes;
using DotNetToolbox.ConsoleApplication.Nodes.Application;

namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

public interface ICommand
    : IExecutableNode, INamedNode {
    IApplication Application { get; }
}
