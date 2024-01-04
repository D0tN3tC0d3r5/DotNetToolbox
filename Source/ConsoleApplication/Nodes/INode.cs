namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INode {
    string Name { get; }
    string[] Aliases { get; }
    string[] Ids { get; }
    string Description { get; }
    void AppendHelp(StringBuilder builder);
    public string GetPath(bool includeApplicationName) => this switch {
        IHasParent hasParent => $"{hasParent.Parent.GetPath(includeApplicationName)} {Name}",
        IApplication when !includeApplicationName => string.Empty,
        _ => Name,
    };
}
