using DotNetToolbox.ConsoleApplication.Application;

namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IHasParent : INode {
    IApplication Application { get; }
    IHasChildren Parent { get; }
};
