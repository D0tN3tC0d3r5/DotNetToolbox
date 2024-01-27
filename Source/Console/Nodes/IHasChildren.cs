namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IHasChildren : INode {
    ICollection<INode> Children { get; }
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public INode[] Options => [.. Children.OfType<IFlag>().Cast<INode>()
                                          .Union(Children.OfType<IOption>())
                                          .Union(Children.OfType<ICommand>().Where(i => i.Name.StartsWith('-')))
                                          .OrderBy(i => i.Name)];
    public INode[] Commands => [.. Children.Except(Options.Union(Parameters))
                                           .OrderBy(i => i.Name)];
}
