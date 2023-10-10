namespace DotNetToolbox.Http.Options;

public class ApiKeyAuthorizationOptions : AuthorizationOptions {
    public string? ApiKey { get; set; }

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(ApiKey))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(ApiKey)));

        return result;
    }
}