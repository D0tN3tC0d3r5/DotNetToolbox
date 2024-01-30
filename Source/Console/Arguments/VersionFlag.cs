namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class VersionFlag
    : Flag<VersionFlag> {
    private readonly VersionCommand _command;

    public VersionFlag(IHasChildren parent)
        : base(parent, "Version", []) {
        _command = new(parent);
        Description = _command.Description;
    }

    protected override Task<Result> Execute(CancellationToken ct = default)
        => _command.Execute(ct);
}
