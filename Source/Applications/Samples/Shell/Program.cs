var app = BigMouth.BigMouth.Create(args, b => {
                                             b.AddAppSettings(); // This will add the values from appsettings.json to the context
                                             b.AddUserSecrets<Program>(); // This will add the values from the user secrets to the context
                                         });

await app.RunAsync();
