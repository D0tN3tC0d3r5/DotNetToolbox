var builder = SophiaShellApplication.CreateBuilder(args, configuration => {
                                                             configuration.AddAppSettings();
                                                             configuration.AddUserSecrets<Program>();
                                                         });

builder.Services.AddOpenAI();

var app = builder.Build();

await app.RunAsync();
