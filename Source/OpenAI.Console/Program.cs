var app = ShellApplication.Create(args, builder
        => builder.AddSettings()
            .AddUserSecrets<Program>());

await app.RunAsync();
