namespace DotNetToolbox.Http.Options;

public class HttpClientOptions
    : NamedOptions<HttpClientOptions>, IValidatable {
    public const string DefaultResponseFormat = "application/json";

    public virtual Uri? BaseAddress { get; set; }

    public virtual string ResponseFormat { get; set; } = DefaultResponseFormat;

    public virtual Dictionary<string, string[]> CustomHeaders { get; set; } = [];

    public virtual AuthenticationOptions? Authentication { get; set; }

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
