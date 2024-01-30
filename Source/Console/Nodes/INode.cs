namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INode {
    string Name { get; }
    string[] Aliases { get; }
    string Description { get; }
    public string Path => this switch {
        IHasParent node => $"{node.Parent.Path} {Name}".Trim(),
        _ =>  ((IApplication)this).AssemblyName,
    };
}
