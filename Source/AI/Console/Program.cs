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
    ab.SetOutputHandler(new ConsoleOutput());
    ab.SetInputHandler(new ConsoleInput());
    ab.Services.Configure<LolaSettings>(ab.Configuration.GetSection("Lola"));
    ab.Services.AddOptions<LolaSettings>();

    ab.Services.AddOpenAI();
    ab.Services.AddAnthropic();

    ab.Services.AddScoped<IHttpConnectionHandler, HttpConnectionHandler>();
    ab.Services.AddScoped(p => new Lazy<IHttpConnectionHandler>(p.GetRequiredService<IHttpConnectionHandler>));

    ab.Services.AddSingleton<IUserProfileRepositoryStrategy, UserProfileRepositoryStrategy>();
    ab.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
    ab.Services.AddScoped<IUserProfileHandler, UserProfileHandler>();
    ab.Services.AddScoped(p => new Lazy<IUserProfileRepository>(p.GetRequiredService<IUserProfileRepository>));
    ab.Services.AddScoped(p => new Lazy<IUserProfileHandler>(p.GetRequiredService<IUserProfileHandler>));

    ab.Services.AddSingleton<IProviderRepositoryStrategy, ProviderRepositoryStrategy>();
    ab.Services.AddScoped<IProviderRepository, ProviderRepository>();
    ab.Services.AddScoped<IProviderHandler, ProviderHandler>();
    ab.Services.AddScoped(p => new Lazy<IProviderRepository>(p.GetRequiredService<IProviderRepository>));
    ab.Services.AddScoped(p => new Lazy<IProviderHandler>(p.GetRequiredService<IProviderHandler>));

    ab.Services.AddSingleton<IModelRepositoryStrategy, ModelRepositoryStrategy>();
    ab.Services.AddScoped<IModelRepository, ModelRepository>();
    ab.Services.AddScoped<IModelHandler, ModelHandler>();
    ab.Services.AddScoped(p => new Lazy<IModelRepository>(p.GetRequiredService<IModelRepository>));
    ab.Services.AddScoped(p => new Lazy<IModelHandler>(p.GetRequiredService<IModelHandler>));

    ab.Services.AddSingleton<IPersonaRepositoryStrategy, PersonaRepositoryStrategy>();
    ab.Services.AddScoped<IPersonaRepository, PersonaRepository>();
    ab.Services.AddScoped<IPersonaHandler, PersonaHandler>();
    ab.Services.AddScoped(p => new Lazy<IPersonaRepository>(p.GetRequiredService<IPersonaRepository>));
    ab.Services.AddScoped(p => new Lazy<IPersonaHandler>(p.GetRequiredService<IPersonaHandler>));

    ab.Services.AddSingleton<ITaskRepositoryStrategy, TaskRepositoryStrategy>();
    ab.Services.AddScoped<ITaskRepository, TaskRepository>();
    ab.Services.AddScoped<ITaskHandler, TaskHandler>();
    ab.Services.AddScoped(p => new Lazy<ITaskRepository>(p.GetRequiredService<ITaskRepository>));
    ab.Services.AddScoped(p => new Lazy<ITaskHandler>(p.GetRequiredService<ITaskHandler>));
});

try {
    await app.RunAsync();
}
finally {
    Log.CloseAndFlush();
}
