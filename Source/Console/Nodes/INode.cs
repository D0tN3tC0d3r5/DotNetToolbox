namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INode {
    string Name { get; }
    string[] Aliases { get; }
    string Description { get; }
    Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default);

    void AppendHelp(StringBuilder builder);
    public string GetPath(bool includeApplication)
        => (this is IHasParent hasParent
               ? $"{hasParent.Parent.GetPath(includeApplication)} {Name}"
               : includeApplication
                   ? Name
                   : string.Empty).Trim();
}
