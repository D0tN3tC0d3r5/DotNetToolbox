var app = ShellApplication.Create(args, builder
        => builder.AddSettings()
            .AddUserSecrets<Program>()
            .SetLogging());

await app.RunAsync();
