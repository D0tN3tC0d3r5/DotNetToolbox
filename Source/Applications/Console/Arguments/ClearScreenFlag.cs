namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class ClearScreenFlag
    : Flag<ClearScreenFlag> {
    private readonly ClearScreenCommand _command;

    public ClearScreenFlag(IHasChildren parent)
        : base(parent, "Clear-Screen", ["cls"]) {
        _command = new(parent);
        Description = _command.Description;
    }

    protected override Task<Result> Execute(CancellationToken ct = default)
        => _command.Execute(ct);
}
