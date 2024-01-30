using static DotNetToolbox.ConsoleApplication.Application.ApplicationBase;

namespace DotNetToolbox.ConsoleApplication.Arguments;

internal class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IHasChildren parent)
        : base(parent, "Exit", []) {
        Description = "Exit the application.";
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        Application.ExitWith(DefaultExitCode);
        return SuccessTask();
    }
}
