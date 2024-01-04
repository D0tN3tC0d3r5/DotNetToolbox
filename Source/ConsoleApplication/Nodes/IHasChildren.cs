namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IHasChildren : INode {
    ICollection<INode> Children { get; }
    public INode[] Options => Children.OfType<IFlag>()
                                      .Cast<INode>()
                                      .Union(Children.OfType<IOption>())
                                      .Union(Children.OfType<IOption>())
                                      .OrderBy(i => i.Name)
                                      .ToArray();
    public IHasValue[] Parameters => Children.OfType<IParameter>()
                                             .OrderBy(i => i.Order)
                                             .Cast<IHasValue>()
                                             .ToArray();
    public IExecutable[] Commands => Children.OfType<ICommand>()
                                             .OrderBy(i => i.Name)
                                             .Cast<IExecutable>()
                                             .ToArray();
}
