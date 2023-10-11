namespace DotNetToolbox.Http.Options;

public class SimpleHttpClientOptions {
    public const string DefaultResponseFormat = "application/json";

    public string BaseAddress { get; set; } = default!;

    public string ResponseFormat { get; set; } = DefaultResponseFormat;

    public Dictionary<string, string[]> CustomHeaders { get; set; } = new();

    public AuthorizationOptions? Authorization { get; set; }

    public void EnsureIsValid(string? message = null)
        => Validate().EnsureIsValid(message);

    public virtual ValidationResult Validate(string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(BaseAddress))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(HttpClientOptions.BaseAddress)));

        if (string.IsNullOrWhiteSpace(ResponseFormat))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(HttpClientOptions.ResponseFormat)));

        result += Authorization?.Validate(httpClientName) ?? Success();

        return result;
    }

    private static string GetSource(string? name, params string[] fields)
        => $"{(name is null ? string.Empty : $"{name}.")}"
         + $"{string.Join(".", fields)}";
}