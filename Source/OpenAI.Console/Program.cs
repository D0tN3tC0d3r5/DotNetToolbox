var app = ShellApplication.Create(args, builder
        => builder.AddAppSettings()
            .AddUserSecrets<Program>());

await app.RunAsync();
