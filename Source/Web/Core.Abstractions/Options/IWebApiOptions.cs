namespace WebApi.Options;

public interface IWebApiOptions<out TOptions>
    : IValidatable
    where TOptions : IWebApiOptions<TOptions> {
    CacheOptions Cache { get; }
    GenerateOpenApiJson GenerateOpenApiJson { get; }
    bool UseTelemetry { get; }
}
