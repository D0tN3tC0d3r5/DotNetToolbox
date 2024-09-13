namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IHasParent
    : INode {
    IHasChildren Parent { get; }
};
