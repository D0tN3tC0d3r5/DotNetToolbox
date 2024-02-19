using DotNetToolbox.Sophia;

var app = Sophia.Create(args, builder => {
    builder.AddAppSettings()
           .AddUserSecrets<Program>();

    builder.Services.AddOpenAI(builder.Configuration);
});

await app.RunAsync();
