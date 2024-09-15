namespace Lola;

public class LolaShellApplication
        : ShellApplication<LolaShellApplication, LolaSettings> {
    private readonly Lazy<IUserProfileHandler> _userHandler;
    private readonly ILogger<LolaShellApplication> _logger;

    public LolaShellApplication(string[] args, IServiceCollection services, Lazy<IUserProfileHandler> userHandler, ILogger<LolaShellApplication> logger)
        : base(args, services) {
        _userHandler = userHandler;
        _logger = logger;
        AddCommand<HelpCommand>();
        AddCommand<SettingsCommand>();
        AddCommand<ProvidersCommand>();
        AddCommand<ModelsCommand>();
        if (_userHandler.Value.CurrentUser is not null) AddCommand<UserProfileCommand>();
        AddCommand<PersonasCommand>();
        AddCommand<TasksCommand>();
    }

    protected override async Task<Result> OnStart(CancellationToken ct = default) {
        _logger.LogInformation("Starting Lola application...");
        var header = new FigletText($"{Name} {DisplayVersion}").LeftJustified().Color(Color.Fuchsia);
        Output.Write(header);

        var user = _userHandler.Value.CurrentUser;
        return user is not null ? SaluteUser(user) : await RegisterUser(ct);
    }

    protected override Result OnExit() {
        AnsiConsole.MarkupLine("[green]Thank you for using Lola. Goodbye![/]");
        return Result.Success();
    }

    protected override Task<Result> ProcessInteraction(CancellationToken ct = default) {
        _logger.LogInformation("Executing default command...");
        var choice = Input.BuildSelectionPrompt<string>("What would you like to do?")
                          .ConvertWith(MapTo)
                          .AddChoices("Providers",
                                      "Models",
                                      "Personas",
                                      "Tasks",
                                      "UserProfile",
                                      "Settings",
                                      "Help",
                                      "Exit").Show();

        return ProcessCommand(choice, ct);

        static string MapTo(string choice) => choice switch {
            "Providers" => "Manage Providers",
            "Models" => "Manage Models",
            "Personas" => "Manage Personas",
            "Tasks" => "Manage Tasks",
            "UserProfile" => "Manage User Profile",
            "Settings" => "Settings",
            "Help" => "Help",
            "Exit" => "Exit",
            _ => string.Empty,
        };
    }

    protected override Task<Result> ProcessCommand(string[] input, CancellationToken ct) {
        _logger.LogInformation("Processing command: '{Command}'...", string.Join(" ", input));
        return base.ProcessCommand(input, ct);
    }

    protected override bool HandleException<TException>(TException ex) {
        _logger.LogError(ex, "An error occurred while executing the app.");
        return base.HandleException(ex);
    }

    private Result SaluteUser(UserProfileEntity user) {
        Output.WriteLine($"[Green]Hi {user.Name}! Welcome back.[/]");
        Output.WriteLine();
        return Result.Success();
    }

    private Task<Result> RegisterUser(CancellationToken ct = default) {
        Output.WriteLine($"[bold]Welcome to {Name}, your AI assisted shell![/]");
        Output.WriteLine();
        var command = new UserProfileSetCommand(this, _userHandler.Value);
        Output.WriteLine("[bold][Yellow]Hi![/] It seems that is the first time that I see you around here.[/]");
        return command.Execute([], ct);
    }
}
