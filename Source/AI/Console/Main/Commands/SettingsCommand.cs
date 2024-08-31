namespace AI.Sample.Main.Commands;

public class SettingsCommand : Command<SettingsCommand> {
    private readonly LolaSettings _settings;
    private readonly ILogger<SettingsCommand> _logger;

    public SettingsCommand(IHasChildren parent, IOptions<LolaSettings> settings, ILogger<SettingsCommand> logger)
        : base(parent, "Settings", ["set"]) {
        _logger = logger;
        _settings = settings.Value;
        Description = "Display the current configuration of Lola.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        _logger.LogInformation("Executing Settings command...");
        var table = new Table();
        table.AddColumn("Setting");
        table.AddColumn("Value");
        table.AddRow("WithDefault AI Provider", _settings.DefaultAIProvider);
        table.AddRow("Available Models", string.Join(", ", _settings.AvailableModels));
        AnsiConsole.Write(table);

        return Result.SuccessTask();
    }
}
