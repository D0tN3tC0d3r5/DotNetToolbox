namespace WebApi.Options;

public record WebApiOptions<TOptions>
    : IWebApiOptions<TOptions>
    where TOptions : WebApiOptions<TOptions>, new() {
    public static TOptions Default => new();

    public CacheOptions Cache { get; set; } = new();
    public CorsOptions Cors { get; set; } = new();
    public GenerateOpenApiJson GenerateOpenApiJson { get; set; } = GenerateOpenApiJson.OnlyInDevelopment;
    public bool UseTelemetry { get; set; } = true;

    public virtual Result Validate(IMap? context = null)
        => Result.Default;
}