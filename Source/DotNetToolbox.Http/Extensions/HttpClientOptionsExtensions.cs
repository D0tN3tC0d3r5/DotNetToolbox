using static DotNetToolbox.Http.Options.HttpClientAuthorizationScheme;

namespace DotNetToolbox.Http.Extensions;

public static class HttpClientOptionsExtensions {
    public static bool IsActive(this HttpClientAuthorizationOptions options) {
        var now = DateTimeOffset.UtcNow;
        return ((options.ExpiresOn == null) || (options.ExpiresOn > now))
            && ((options.NotBefore == null) || (options.NotBefore < now));
    }

    public static void EnsureIsValid(this HttpClientBasicOptions options, string? message = null)
        => options.Validate().EnsureIsValid(message);

    internal static ValidationResult Validate(this HttpClientBasicOptions options, string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(options.BaseAddress))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(HttpClientOptions.BaseAddress)));

        if (string.IsNullOrWhiteSpace(options.ResponseFormat))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(HttpClientOptions.ResponseFormat)));

        result += options.Authorization?.Validate() ?? Success();

        return result;
    }

    private static ValidationResult Validate(this HttpClientAuthorizationOptions options, string? httpClientName = null) {
        var result = Success();

        result += options.Type switch {
            HttpClientAuthorizationType.ApiKey => options.ValidateForApiKey(httpClientName),
            _ => Success(),
        };

        return result;
    }

    private static ValidationResult ValidateForApiKey(this HttpClientAuthorizationOptions options, string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(options.Value))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(HttpClientAuthorizationOptions.Value)));

        return result;
    }

    private static ValidationResult ValidateForJsonWebToken(this HttpClientAuthorizationOptions options, string? httpClientName = null) {
        var result = Success();

        if (options.Scheme != Bearer)
            result += new ValidationError(MustBe, GetSource(httpClientName, nameof(HttpClientAuthorizationOptions.Scheme)), nameof(Bearer), options.Scheme.ToString());
        if (string.IsNullOrWhiteSpace(options.ClientSecret))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(HttpClientAuthorizationOptions.ClientSecret)));

        return result;
    }

    private static string GetSource(string? name, params string[] fields)
        => $"{(name is null ? string.Empty : $"{name}.")}"
         + $"{string.Join(".", fields)}";
}