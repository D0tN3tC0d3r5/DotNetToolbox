var app = ShellApplication.Create(args, builder => {
                                            builder.AddAppSettings()
                                                   .AddUserSecrets<Program>();

                                            builder.Services.AddOpenAI(builder.Configuration);
                                        });

app.AddCommand<StartChatCommand>();
app.AddCommand<EndChatCommand>();
app.AddCommand<SendMessageCommand>();

await app.RunAsync();
