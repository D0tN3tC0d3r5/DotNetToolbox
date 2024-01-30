namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpFlag
    : Flag<HelpFlag> {
    private readonly HelpCommand _command;

    public HelpFlag(IHasChildren parent)
        : base(parent, "Help", ["h", "?"]) {
        _command = new(parent);
        Description = _command.Description;
    }

    protected override Task<Result> Execute(CancellationToken ct = default)
        => _command.Execute(ct);
}
