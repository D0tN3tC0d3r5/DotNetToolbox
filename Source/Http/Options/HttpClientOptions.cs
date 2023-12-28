namespace DotNetToolbox.Http.Options;

public class HttpClientOptions : IValidatable  {
    public const string DefaultResponseFormat = "application/json";

    public Dictionary<string, HttpClientOptions>? Clients { get; set; }

    public Uri? BaseAddress { get; set; }

    public string ResponseFormat { get; set; } = DefaultResponseFormat;

    public Dictionary<string, string[]> CustomHeaders { get; set; } = [];

    public AuthenticationOptions? Authentication { get; set; }

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Success();

        if (BaseAddress is null)
            result += new ValidationError(GetSourcePath(nameof(BaseAddress)), ValueCannotBeNullOrWhiteSpace);

        if (string.IsNullOrWhiteSpace(ResponseFormat))
            result += new ValidationError(GetSourcePath(nameof(ResponseFormat)), ValueCannotBeNullOrWhiteSpace);

        result += Authentication?.Validate(context) ?? Success();

        if (Clients is null) return result;

        foreach (var client in Clients) {
            var clientContext = new Dictionary<string, object?> { ["ClientName"] = GetSourcePath(client.Key) };
            result += client.Value.Validate(clientContext);
        }

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }
}
