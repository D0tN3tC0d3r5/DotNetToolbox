using System.Results;

namespace DotNetToolbox.Http.Options;

public class HttpClientBasicOptions {
    public string? BaseAddress { get; set; }

    public string? ResponseFormat { get; set; } = "application/json";

    public HttpClientAuthorizationOptions? Authorization { get; set; }

    public Dictionary<string, string[]> CustomHeaders { get; set; } = new();

    public void EnsureIsValid(string? message = null)
        => Validate().EnsureIsValid(message);

    public ValidationResult Validate(string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(BaseAddress))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(HttpClientOptions.BaseAddress)));

        if (string.IsNullOrWhiteSpace(ResponseFormat))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(HttpClientOptions.ResponseFormat)));

        result += Authorization?.Validate() ?? Success();

        return result;
    }

    private static string GetSource(string? name, params string[] fields)
        => $"{(name is null ? string.Empty : $"{name}.")}"
         + $"{string.Join(".", fields)}";
}