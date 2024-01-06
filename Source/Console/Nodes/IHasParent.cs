namespace ConsoleApplication.Nodes;

public interface IHasParent : INode {
    IApplication Application { get; }
    IHasChildren Parent { get; }
};
