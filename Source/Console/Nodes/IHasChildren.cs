namespace ConsoleApplication.Nodes;

public interface IHasChildren : INode {
    ICollection<INode> Children { get; }
    public INode[] Options => [.. Children.OfType<IFlag>()
                                      .Cast<INode>()
                                      .Union(Children.OfType<IOption>())
                                      .Union(Children.OfType<IAction>())
                                      .OrderBy(i => i.Name)];
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IExecutable[] Commands => Children.OfType<ICommand>()
                                             .OrderBy(i => i.Name)
                                             .Cast<IExecutable>()
                                             .ToArray();
}
