namespace DotNetToolbox.Http.Options;

public class ApiKeyAuthenticationOptions : AuthenticationOptions {
    private const string _apiKeyHeaderKey = "x-api-key";

    public AuthenticationScheme Scheme { get; set; } = Bearer;
    public string ApiKey { get; set; } = string.Empty;

    public override Result Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate(context);

        if (string.IsNullOrWhiteSpace(ApiKey))
            result += new ValidationError(StringCannotBeNullOrWhiteSpace, GetSourcePath(nameof(ApiKey)));

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }

    public override HttpAuthentication Configure(HttpClient client, HttpAuthentication _) {
        var authentication = new HttpAuthentication {
            Type = AuthenticationType.ApiKey,
            Value = ApiKey,
            Scheme = Scheme,
        };
        client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, authentication.Value);
        return authentication;
    }
}
