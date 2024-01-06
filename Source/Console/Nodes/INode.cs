namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INode {
    string Name { get; }
    string[] Ids { get; }
    string Description { get; }
    void AppendHelp(StringBuilder builder);
    public string GetPath(bool includeApplication)
        => (this is IHasParent hasParent
               ? $"{hasParent.Parent.GetPath(includeApplication)} {Name}"
               : includeApplication
                   ? Name
                   : string.Empty).Trim();
}
