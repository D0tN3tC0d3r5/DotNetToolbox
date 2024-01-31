var app = CommandLineInterfaceApplication.Create(args, builder
        => builder.AddSettings()
                  .AddUserSecrets<Program>());

app.AddCommand<SayCommand>();

await app.RunAsync();
