namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpCommand
    : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", ["?"]) {
        _parent = parent;
        Description = "Display this help information.";
        AddParameter("Target", string.Empty);
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var target = (string?)Context.GetValueOrDefault("Target");
        var command = _parent.Commands.FirstOrDefault(i => i.Name.Equals(target, StringComparison.CurrentCultureIgnoreCase));
        var help = FormatHelp(command ?? _parent);
        Environment.Output.WriteLine(help);
        return SuccessTask();
    }
}
