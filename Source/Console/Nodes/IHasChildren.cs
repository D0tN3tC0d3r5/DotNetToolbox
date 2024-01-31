namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface IHasChildren : INode {
    public NodeContext Context { get; }

    ICollection<INode> Children { get; }
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    ICommand AddCommand(string name, Delegate action);
    ICommand AddCommand(string name, string alias, Delegate action);
    ICommand AddCommand(string name, string[] aliases, Delegate action);
    ICommand AddCommand<TChildCommand>() where TChildCommand : Command<TChildCommand>, ICommand;

    IFlag AddFlag(string name, Delegate? action = null);
    IFlag AddFlag(string name, string alias, Delegate? action = null);
    IFlag AddFlag(string name, string[] aliases, Delegate? action = null);
    IFlag AddFlag<TFlag>() where TFlag : Flag<TFlag>, IFlag;

    IOption AddOption(string name);
    IOption AddOption(string name, string alias);
    IOption AddOption(string name, string[] aliases);
    IOption AddOption<TOption>() where TOption : Option<TOption>, IOption;

    IParameter AddParameter(string name);
    IParameter AddParameter(string name, string? defaultValue);
    IParameter AddParameter<TParameter>() where TParameter : Parameter<TParameter>, IParameter;
}
