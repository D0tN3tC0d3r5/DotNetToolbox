namespace DotNetToolbox.Http.Options;

public record HttpClientOptions
    : INamedOptions<HttpClientOptions>, IValidatable {
    public const string DefaultResponseFormat = "application/json";

    public static string SectionName => "HttpClient";
    public static HttpClientOptions Default => new();

    public virtual Uri? BaseAddress { get; init; }

    public virtual string ResponseFormat { get; init; } = DefaultResponseFormat;

    public virtual Dictionary<string, string[]> CustomHeaders { get; init; } = [];

    public virtual AuthenticationOptions? Authentication { get; init; }

    public virtual Result Validate(IDictionary<string, object?>? context = null) {
        var result = Success();

        if (BaseAddress is null)
            result += new ValidationError(StringCannotBeNullOrWhiteSpace, GetSourcePath(nameof(BaseAddress)));

        result += Authentication?.Validate(context) ?? Success();
        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }
}
