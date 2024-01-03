using DotNetToolbox.ConsoleApplication.Nodes.Application;

namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IApplication application)
        : base(application, "Exit") {
    }

    protected override async Task<Result> ExecuteAsync(CancellationToken ct) {
        await Application.ExitAsync();
        return Result.Success();
    }
}
