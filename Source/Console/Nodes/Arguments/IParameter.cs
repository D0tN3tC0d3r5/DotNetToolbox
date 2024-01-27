namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IParameter : IHasParent {
    bool IsSet { get; }
    bool IsRequired { get; }
    int Order { get; }
}
