namespace WebApi.Builders;

public class MultiTenantWebApiBuilder<TTenantDataStore>(string[] args)
    : MultiTenantWebApiBuilder<TTenantDataStore, Tenant>(args)
    where TTenantDataStore : class, ITenantStore<Tenant>;

public class MultiTenantWebApiBuilder<TTenantDataStore, TTenant>(string[] args)
    : MultiTenantWebApiBuilder<MultiTenantWebApiOptions, TTenantDataStore, TTenant>(args)
    where TTenantDataStore : class, ITenantStore<TTenant>
    where TTenant : Tenant, new();

public class MultiTenantWebApiBuilder<TOptions, TTenantDataStore, TTenant>
    : WebApiBuilder<TOptions>
    where TOptions : MultiTenantWebApiOptions<TOptions>, new()
    where TTenantDataStore : class, ITenantStore<TTenant>
    where TTenant : Tenant, new() {
    public MultiTenantWebApiBuilder(string[] args)
        : base(args) {
        Services.AddScoped<ITenantStore<TTenant>, TTenantDataStore>();
        Services.AddScoped<ICurrentTenantAccessor, CurrentTenantAccessor>();
        Services.AddScoped<ITenantManagementService, TenantManagementService>();

        Services.AddAuthentication(o => {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            var serviceProvider = Services.BuildServiceProvider();
            var tenantOptions = serviceProvider.GetRequiredService<IOptions<MultiTenantWebApiOptions>>().Value;
            var result = tenantOptions.Validate();
            if (result.HasErrors) {
                var errorMessages = string.Join("", $"{System.Environment.NewLine} - ${result.Errors.Select(e => e.ToString())}");
                throw new InvalidOperationException($"Invalid TenantOptions configuration:{errorMessages}");
            }

            var accessTokenOptions = tenantOptions.TenantAccessToken;
            var keyBytes = Convert.FromBase64String(accessTokenOptions.Key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            options.TokenValidationParameters = new() {
                ValidateIssuer = true,
                ValidIssuer = accessTokenOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = accessTokenOptions.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.FromSeconds(30),
            };
            options.Events = new() {
                OnAuthenticationFailed = context => {
                    var loggerFactory = context.HttpContext.RequestServices.GetService<ILoggerFactory>();
                    var logger = loggerFactory?.CreateLogger($"{typeof(MultiTenantWebApiBuilder<>).Name}.{nameof(JwtBearerEvents.OnAuthenticationFailed)}");
                    logger?.LogError(context.Exception, "JWT Authentication Failed.");
                    return Task.CompletedTask;
                },
                OnTokenValidated = _ => Task.CompletedTask,
            };
        });

        Services.AddAuthorization();
    }

    public override WebApplication Build(Action<WebApplication>? configure = null) {
        base.Build(configure);
        var app = base.Build(configure);
        app.UseTenantContext();
        return app;
    }
}
