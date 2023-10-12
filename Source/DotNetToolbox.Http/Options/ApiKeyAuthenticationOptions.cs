namespace DotNetToolbox.Http.Options;

public record ApiKeyAuthenticationOptions : AuthenticationOptions {
    private const string _apiKeyHeaderKey = "x-api-key";

    public ApiKeyAuthenticationOptions() {
    }

    [SetsRequiredMembers]
    public ApiKeyAuthenticationOptions(IConfiguration config)
        : this() {
        ApiKey = IsNotNullOrWhiteSpace(config.GetValue<string>(nameof(ApiKey)));
    }

    public string ApiKey { get; set; } = string.Empty;

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(ApiKey))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(ApiKey)));

        return result;
    }

    internal override void Configure(HttpClient client, ref HttpClientAuthentication authentication) {
        authentication = new() {
            Type = AuthenticationType.ApiKey,
            Value = ApiKey,
        };
        client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, authentication.Value);
    }
}