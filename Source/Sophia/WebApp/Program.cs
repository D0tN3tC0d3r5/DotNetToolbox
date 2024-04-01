var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<UserAccessor>();
builder.Services.AddScoped<Redirect>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

builder.Services.AddTransient<IUserAccessor, WebAppUserAccessor>();
builder.Services.AddRepositories(builder.Configuration,
                                 builder.Environment,
                                 "Sophia.WebApp",
                                 identity => identity.AddSignInManager()
                                                     .AddDefaultTokenProviders());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();
if (builder.Environment.IsDevelopment())
    builder.WebHost.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");

builder.Services.AddAnthropic();
builder.Services.AddOpenAI();
builder.Services.AddServices();
builder.Services.AddRemoteServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

await using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<DataContext>().EnsureIsUpToDate();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(Sophia.WebClient._Imports).Assembly);

app.MapIdentityEndpoints();
app.MapProvidersEndpoints();
app.MapWorldEndpoints();
app.MapToolsEndpoints();
app.MapPersonasEndpoints();
app.MapChatsEndpoints();
app.MapAgentEndpoints();

await app.RunAsync();
