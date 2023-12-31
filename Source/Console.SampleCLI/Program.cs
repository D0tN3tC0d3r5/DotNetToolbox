﻿var app = CommandLineInterfaceApplication.Create(args, builder
                                                => builder.AddSettings()
                                                          .AddUserSecrets<Program>()
                                                          .SetLogging());

app.AddCommand<SayCommand>();

await app.RunAsync();
