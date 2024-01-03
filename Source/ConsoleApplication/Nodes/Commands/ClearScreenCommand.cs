using DotNetToolbox.ConsoleApplication.Nodes.Application;

namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class ClearScreenCommand : Command<ClearScreenCommand> {
    public ClearScreenCommand(IApplication application)
        : base(application, "ClearScreen") {
        Alias = "cls";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        Output.ClearScreen();
        return Result.SuccessTask();
    }
}
