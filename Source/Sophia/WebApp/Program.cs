using Sophia.Models.Users;

using UserData = Sophia.Models.Users.UserData;

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

var connectionString = builder.Configuration.GetConnectionString("Sophia.WebApp")
                    ?? throw new InvalidOperationException("Connection string 'Sophia.WebApp' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
    options.LogTo(Console.WriteLine, LogLevel.Information);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<UserData>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<UserData>, IdentityNoOpEmailSender>();
if (builder.Environment.IsDevelopment())
    builder.WebHost.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");

builder.Services.AddAnthropic();
builder.Services.AddOpenAI();
builder.Services.AddScoped<IWorldService, WorldService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProvidersService, ProvidersService>();
builder.Services.AddScoped<IToolsService, ToolsService>();
builder.Services.AddScoped<IPersonasService, PersonasService>();
builder.Services.AddScoped<IChatsService, ChatsService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IWorldRemoteService, WorldRemoteService>();
builder.Services.AddScoped<IProvidersRemoteService, ProvidersRemoteService>();
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
app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

await app.Services.GetRequiredService<DataContext>().EnsureIsUpToDate();

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
