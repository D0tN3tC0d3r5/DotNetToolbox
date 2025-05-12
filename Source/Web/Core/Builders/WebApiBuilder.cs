namespace WebApi.Builders;

public abstract class WebApiBuilder<TOptions>
    : IWebApiBuilder<WebApplication, TOptions>
    where TOptions : WebApiOptions<TOptions>, new() {
    private readonly WebApplicationBuilder _builder;

    protected WebApiBuilder(string[] args) {
        _builder = WebApplication.CreateSlimBuilder(args);
        _builder.Host.UseDefaultServiceProvider(static (_, o) => {
            o.ValidateScopes = true;
            o.ValidateOnBuild = true;
        });
        AddRequiredServices();
        AddTelemetry();
        AddCaching();
        AddCors();
        AddOpenApi();
    }

    public TOptions Options { get; } = new();
    public IServiceCollection Services => _builder.Services;
    public IConfigurationManager Configuration => _builder.Configuration;
    public IHostEnvironment Environment => _builder.Environment;
    public ILoggingBuilder Logging => _builder.Logging;
    public IMetricsBuilder Metrics => _builder.Metrics;

    public virtual WebApplication Build(Action<WebApplication>? configure = null) {
        var app = _builder.Build();
        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseRouting();
        if (!Options.Cors.Disabled)
            app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHealthCheckEndpoints();

        if (HasOpenApi)
            app.MapOpenApi();

        configure?.Invoke(app);
        return app;
    }

    private bool HasOpenApi => Options.GenerateOpenApiJson == GenerateOpenApiJson.Yes
                            || (Options.GenerateOpenApiJson == GenerateOpenApiJson.OnlyInDevelopment && Environment.IsDevelopment());

    IConfigurationManager IHostApplicationBuilder.Configuration => _builder.Configuration;
    IHostEnvironment IHostApplicationBuilder.Environment => Environment;
    void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure)
        => ((IHostApplicationBuilder)_builder).ConfigureContainer(factory, configure);
    IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_builder).Properties;

    private void AddRequiredServices() {
        _builder.Services.AddServiceDiscovery();
        AddDefaultHealthChecks();
        _builder.Services.ConfigureHttpClientDefaults(static http => {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });
        _builder.Services.AddOptionsWithValidateOnStart<TOptions>()
                .Bind(_builder.Configuration)
                .ValidateDataAnnotations();
        _builder.Services.AddProblemDetails();
        _builder.Configuration.Bind(Options);
        _builder.Services.AddHttpContextAccessor();
        _builder.Services.AddSingleton(TimeProvider.System);
        _builder.Services.Configure<JsonOptions>(static o => o.SerializerOptions.Converters.Add(new OptionalConverterFactory()));
        _builder.Services.AddSingleton<ITokenFactory, TokenFactory>();
    }

    private void AddCors() {
        if (Options.Cors.Disabled)
            return;
        _builder.Services.AddCors();
    }

    private void AddCaching() {
        switch (Options.Cache.Type) {
            case CacheType.Redis:
                _builder.AddRedisDistributedCache("redis", opt => {
                    opt.ConnectionString = string.IsNullOrEmpty(Options.Cache.ConnectionString)
                                                ? opt.ConnectionString
                                                : Options.Cache.ConnectionString;
                    opt.DisableTracing = !Options.UseTelemetry;
                });
                break;
            case CacheType.Memory:
                _builder.Services.AddDistributedMemoryCache();
                break;
            case CacheType.Custom:
            default:
                break;
        }
        _builder.Services.TryAddSingleton<ICacheService, CacheService>();
    }

    private void AddTelemetry() {
        if (!Options.UseTelemetry)
            return;
        ConfigureOpenTelemetry();
    }

    private void AddOpenApi() {
        if (!HasOpenApi)
            return;
        _builder.Services.AddOpenApi();
    }

    private void ConfigureOpenTelemetry() {
        _builder.Logging.AddOpenTelemetry(logging => {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        _builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation())
            .WithTracing(tracing => tracing.AddSource(_builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation());

        AddOpenTelemetryExporters();
    }

    private void AddOpenTelemetryExporters() {
        // ReSharper disable StringLiteralTypo
        var useExporter = !string.IsNullOrWhiteSpace(_builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        // ReSharper enable StringLiteralTypo

        if (useExporter)
            _builder.Services.AddOpenTelemetry().UseOtlpExporter();
    }

    private void AddDefaultHealthChecks()
        => _builder.Services
                    .AddHealthChecks()
                    .AddCheck("self", static () => HealthCheckResult.Healthy(), ["live"]);
}
