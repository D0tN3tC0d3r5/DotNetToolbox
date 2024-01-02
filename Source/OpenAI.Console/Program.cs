var builder = ShellApplication.CreateBuilder(args)
                              .AddSettings()
                              .AddUserSecrets<Program>()
                              .SetLogging();

var app = builder.Build();

await app.RunAsync();
