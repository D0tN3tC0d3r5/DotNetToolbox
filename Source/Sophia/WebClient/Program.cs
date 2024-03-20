var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddScoped<IWorldRemoteService, WorldRemoteService>();
builder.Services.AddScoped<IToolsRemoteService, ToolsRemoteService>();
builder.Services.AddScoped<IPersonasRemoteService, PersonasRemoteService>();
builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new(builder.Configuration["FrontendUrl"] ?? "https://localhost:7100"),
});

await builder.Build().RunAsync();
