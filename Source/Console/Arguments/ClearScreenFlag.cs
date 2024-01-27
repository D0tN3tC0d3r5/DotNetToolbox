namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class ClearScreenFlag
    : Command<ClearScreenFlag>, IFlag {
    public ClearScreenFlag(IHasChildren parent)
        : base(parent, "ClearScreen", "cls") {
        Description = "Clear the screen before the application starts.";
    }

    protected override Task<Result> Execute() {
        Application.Output.ClearScreen();
        return SuccessTask();
    }
}
