namespace DotNetToolbox.ConsoleApplication.Nodes.Commands;

internal class ExitCommand : Command<ExitCommand> {
    public ExitCommand(IApplication application)
        : base(application, "Exit") {
        Description = "Exit the application.";
    }

    protected override async Task<Result> ExecuteAsync(CancellationToken ct) {
        await Application.ExitAsync();
        return Success();
    }
}
