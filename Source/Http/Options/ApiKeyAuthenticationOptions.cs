namespace DotNetToolbox.Http.Options;

public class ApiKeyAuthenticationOptions : AuthenticationOptions {
    private const string _apiKeyHeaderKey = "x-api-key";

    public string ApiKey { get; set; } = string.Empty;

    internal override Result Validate() {
        var result = base.Validate();

        if (string.IsNullOrWhiteSpace(ApiKey))
            result += new ValidationError(nameof(ApiKey), ValueCannotBeNullOrWhiteSpace);

        return result;
    }

    internal override void Configure(HttpClient client, ref HttpAuthentication authentication) {
        authentication = new() {
            Type = AuthenticationType.ApiKey,
            Value = ApiKey,
        };
        client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, authentication.Value);
    }
}