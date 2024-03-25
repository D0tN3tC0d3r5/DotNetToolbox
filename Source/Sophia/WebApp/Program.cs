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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
    options.LogTo(Console.WriteLine, LogLevel.Information);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
if (builder.Environment.IsDevelopment()) {
    builder.WebHost.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
}

builder.Services.AddAnthropic();
builder.Services.AddOpenAI();
builder.Services.AddScoped<IWorldService, WorldService>();
builder.Services.AddScoped<IProvidersService, ProvidersService>();
builder.Services.AddScoped<IToolsService, ToolsService>();
builder.Services.AddScoped<IPersonasService, PersonasService>();
builder.Services.AddScoped<IChatsService, ChatsService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IProvidersRemoteService, ProvidersRemoteService>();
builder.Services.AddScoped<IWorldRemoteService, WorldRemoteService>();
builder.Services.AddScoped<IPersonasRemoteService, PersonasRemoteService>();
builder.Services.AddScoped<IToolsRemoteService, ToolsRemoteService>();
builder.Services.AddScoped<IChatsRemoteService, ChatsRemoteService>();
builder.Services.AddScoped<IAgentRemoteService, AgentRemoteService>();
builder.Services.AddScoped(_ => new HttpClient {
    BaseAddress = new(builder.Configuration["FrontendUrl"] ?? "https://localhost:7100"),
});

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

await ApplicationDbContext.Seed(app.Services);

app.UseStaticFiles();
app.UseAntiforgery();

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
