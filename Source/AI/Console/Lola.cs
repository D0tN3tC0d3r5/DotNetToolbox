namespace AI.Sample;

public class Lola
        : ShellApplication<Lola, LolaSettings> {
    private readonly ILogger<Lola> _logger;

    public Lola(string[] args, IServiceProvider services, IOptions<LolaSettings> settings, ILogger<Lola> logger)
        : base(args, services) {
        _logger = logger;
        AddCommand<HelpCommand>();
        AddCommand<SettingsCommand>();
        AddCommand<VersionCommand>();
    }

    protected override Task<Result> OnStart(CancellationToken ct = default) {
        _logger.LogInformation("Starting Lola application...");
        AnsiConsole.Write(new FigletText($"{Name} {DisplayVersion}").Centered().Color(Color.Fuchsia));
        AnsiConsole.MarkupLine($"[bold]Welcome to {Name}, your AI Assistant Shell![/]");
        AnsiConsole.MarkupLine("Type 'help' to see available commands.");
        return Result.SuccessTask();
    }

    protected override Task<Result> ExecuteDefault(CancellationToken ct = default) {
        _logger.LogInformation("Executing default command...");
        return base.ExecuteDefault(ct);
    }

    protected override Task<Result> ProcessCommand(string[] input, CancellationToken ct) {
        _logger.LogInformation("Processing command: '{Command}'...", string.Join(" ", input));
        return base.ProcessCommand(input, ct);
    }

    protected override bool HandleException<TException>(TException ex) {
        _logger.LogError(ex, "An error occurred while executing the app.");
        return base.HandleException(ex);
    }
}
