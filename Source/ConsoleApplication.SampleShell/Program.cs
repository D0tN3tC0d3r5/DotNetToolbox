var app = ShellApplication
   .Create(args, builder
        => builder.AddSettings()
               .AddUserSecrets<Program>()
               .SetLogging());

app.Commands.Add(new ClearCommand(app));

await app.RunAsync();
