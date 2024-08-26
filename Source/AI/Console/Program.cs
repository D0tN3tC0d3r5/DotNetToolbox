var app = Lola.Create(args, cb => {
    cb.AddAppSettings(); // This will add the values from appsettings.json to the context
    cb.AddUserSecrets<Program>(); // This will add the values from the user secrets to the context
}, ab => {
    ab.ConfigureLogging((loggingBuilder) => {
        var logPath = Path.Combine("logs", "lola-.log");
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(ab.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(logPath,
                fileSizeLimitBytes: 5 * 1024 * 1024,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7) // 5MB file size limit
            .CreateLogger();

        loggingBuilder.AddSerilog(dispose: true);
    });
    ab.SetOutputHandler(new AnsiOutput());
    ab.Services.Configure<LolaSettings>(ab.Configuration.GetSection("Lola"));
    ab.Services.AddOptions<LolaSettings>();

    ab.Services.AddSingleton<IRepository<Agent, string>>(sp => {
        var filePath = Path.Combine(AppContext.BaseDirectory, "data", "agents.json");
        var strategy = new JsonFileRepositoryStrategy<Agent, string>(filePath);
        return new Repository<JsonFileRepositoryStrategy<Agent, string>, Agent, string>(strategy);
    });
});

try {
    await app.RunAsync();
}
finally {
    Log.CloseAndFlush();
}
