using Sophia.WebClient.Account;
using Sophia.WebClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddScoped<IWorldService, WorldService>();

builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new(builder.Configuration["FrontendUrl"] ?? "https://localhost:7100"),
});

await builder.Build().RunAsync();
