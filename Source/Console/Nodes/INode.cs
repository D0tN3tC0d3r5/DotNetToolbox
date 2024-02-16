namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INode {
    string Name { get; }
    string[] Aliases { get; }
    string Description { get; }
    IApplication Application { get; }
    IEnvironment Environment { get; }
    IQuestionFactory Ask { get; }

    public string Path => this switch {
                              IApplication app => app.AssemblyName,
                              IHasParent { Parent: not IRunAsShell } node => $"{node.Parent.Path} {Name}".Trim(),
                              _ => Name,
                          };
}
