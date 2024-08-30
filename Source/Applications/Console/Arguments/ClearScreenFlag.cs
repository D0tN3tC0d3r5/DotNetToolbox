namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class ClearScreenFlag
    : Flag<ClearScreenFlag> {
    public ClearScreenFlag(IHasChildren parent)
        : base(parent, "Clear-Screen", ["cls"]) {
        Description = "Clear the screen.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        Output.ClearScreen();
        return SuccessTask();
    }
}
