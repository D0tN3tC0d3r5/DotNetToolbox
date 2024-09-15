var app = LolaShellApplication.Create(args, cb => {
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

    ab.Services.AddSingleton<IUserProfileStorage, UserProfileStorage>();
    ab.Services.AddScoped<IUserProfileDataSource, UserProfileDataSource>();
    ab.Services.AddScoped<IUserProfileHandler, UserProfileHandler>();
    ab.Services.AddScoped(p => new Lazy<IUserProfileDataSource>(p.GetRequiredService<IUserProfileDataSource>));
    ab.Services.AddScoped(p => new Lazy<IUserProfileHandler>(p.GetRequiredService<IUserProfileHandler>));

    ab.Services.AddSingleton<IProviderStorage, ProviderStorage>();
    ab.Services.AddScoped<IProviderDataSource, ProviderDataSource>();
    ab.Services.AddScoped<IProviderHandler, ProviderHandler>();
    ab.Services.AddScoped(p => new Lazy<IProviderDataSource>(p.GetRequiredService<IProviderDataSource>));
    ab.Services.AddScoped(p => new Lazy<IProviderHandler>(p.GetRequiredService<IProviderHandler>));

    ab.Services.AddSingleton<IModelStorage, ModelStorage>();
    ab.Services.AddScoped<IModelDataSource, ModelDataSource>();
    ab.Services.AddScoped<IModelHandler, ModelHandler>();
    ab.Services.AddScoped(p => new Lazy<IModelDataSource>(p.GetRequiredService<IModelDataSource>));
    ab.Services.AddScoped(p => new Lazy<IModelHandler>(p.GetRequiredService<IModelHandler>));

    ab.Services.AddSingleton<IPersonaStorage, PersonaStorage>();
    ab.Services.AddScoped<IPersonaDataSource, PersonaDataSource>();
    ab.Services.AddScoped<IPersonaHandler, PersonaHandler>();
    ab.Services.AddScoped(p => new Lazy<IPersonaDataSource>(p.GetRequiredService<IPersonaDataSource>));
    ab.Services.AddScoped(p => new Lazy<IPersonaHandler>(p.GetRequiredService<IPersonaHandler>));

    ab.Services.AddSingleton<ITaskStorage, TaskStorage>();
    ab.Services.AddScoped<ITaskDataSource, TaskDataSource>();
    ab.Services.AddScoped<ITaskHandler, TaskHandler>();
    ab.Services.AddScoped(p => new Lazy<ITaskDataSource>(p.GetRequiredService<ITaskDataSource>));
    ab.Services.AddScoped(p => new Lazy<ITaskHandler>(p.GetRequiredService<ITaskHandler>));
});

try {
    await app.RunAsync();
}
finally {
    Log.CloseAndFlush();
}
