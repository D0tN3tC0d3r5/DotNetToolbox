using static DotNetToolbox.Http.Options.HttpClientAuthorizationScheme;

using Opt = DotNetToolbox.Http.Options.HttpClientOptions;
using AOp = DotNetToolbox.Http.Options.HttpClientAuthorizationOptions;

namespace DotNetToolbox.Http.Extensions;

public static class HttpClientOptionsExtensions {
    public static bool IsActive(this AOp options) {
        var now = DateTimeOffset.UtcNow;
        return ((options.ExpiresOn == null) || (options.ExpiresOn > now))
            && ((options.NotBefore == null) || (options.NotBefore < now));
    }

    public static ValidationResult Validate(this Opt options) {
        var result = ((HttpClientBasicOptions)options).Validate();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator - Better readability
        foreach (var namedOptions in options.Named) {
            result += namedOptions.Value.Validate(namedOptions.Key);
        }

        return result;
    }

    internal static ValidationResult Validate(this HttpClientBasicOptions options, string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(options.BaseAddress))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(Opt.BaseAddress)));

        if (string.IsNullOrWhiteSpace(options.ResponseFormat))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(Opt.ResponseFormat)));

        result += options.Authorization?.Validate() ?? Success();

        return result;
    }

    private static ValidationResult Validate(this AOp options, string? httpClientName = null) {
        var result = Success();

        result += options.Type switch {
            HttpClientAuthorizationType.ApiKey => options.ValidateForApiKey(httpClientName),
            _ => Success(),
        };

        return result;
    }

    private static ValidationResult ValidateForApiKey(this AOp options, string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(options.Value))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(AOp.Value)));

        return result;
    }

    private static ValidationResult ValidateForJsonWebToken(this AOp options, string? httpClientName = null) {
        var result = Success();

        if (options.Scheme != Bearer)
            result += new ValidationError(MustBe, GetSource(httpClientName, nameof(AOp.Scheme)), nameof(Bearer), options.Scheme.ToString());
        if (string.IsNullOrWhiteSpace(options.ClientSecret))
            result += new ValidationError(CannotBeNull, GetSource(httpClientName, nameof(AOp.ClientSecret)));

        return result;
    }

    private static string GetSource(string? name, params string[] fields)
        => $"{nameof(HttpClientOptions)}"
         + $"{(name is null ? string.Empty : $"[{name}]")}"
         + $".{string.Join(".", fields)}";
}