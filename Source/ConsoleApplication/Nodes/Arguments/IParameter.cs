namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IParameter : IArgument, IHasValue {
    int Order { get; }
    object? DefaultValue { get; }
}
