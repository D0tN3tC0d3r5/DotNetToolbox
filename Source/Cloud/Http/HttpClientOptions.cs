namespace DotNetToolbox.Http;

public record HttpClientOptions
    : INamedOptions<HttpClientOptions>, IValidatable {
    public const string DefaultResponseFormat = "application/json";

    public static string SectionName => "HttpClient";
    public static HttpClientOptions Default => new();

    public string BaseAddress { get; init; } = string.Empty;
    public string ResponseFormat { get; init; } = DefaultResponseFormat;
    public HttpClientAuthentication? Authentication { get; set; }
    public Dictionary<string, string[]>? CustomHeaders { get; set; }

    public virtual Result Validate(IDictionary<string, object?>? context = null) {
        var result = new Result();
        var provider = (string?)context?["Provider"] ?? string.Empty;
        var providerPath = string.IsNullOrWhiteSpace(provider) ? string.Empty : $":{provider}";

        result += ValidateBaseAddress(providerPath);
        result += ValidateResponseFormat(providerPath);
        result += ValidateAuthentication(providerPath);
        return result;
    }

    private Result ValidateBaseAddress(string providerPath) {
        var result = new Result();
        if (string.IsNullOrWhiteSpace(BaseAddress))
            result += new ValidationError("Http client base address is missing.", GetConfigurationPath(providerPath, nameof(BaseAddress)));
        else if (!Uri.IsWellFormedUriString(BaseAddress, UriKind.Absolute))
            result += new ValidationError("Http client base address is not a valid URI.", GetConfigurationPath(providerPath, nameof(BaseAddress)));
        return result;
    }

    private Result ValidateResponseFormat(string providerPath) {
        var result = new Result();
        if (string.IsNullOrWhiteSpace(ResponseFormat) && !MediaTypeWithQualityHeaderValue.TryParse(ResponseFormat, out _))
            result += new ValidationError("Http client response format value is not valid.", GetConfigurationPath(providerPath, nameof(ResponseFormat)));
        return result;
    }

    private Result ValidateAuthentication(string providerPath) {
        var result = new Result();
        if (Authentication is null) return result;
        switch (Authentication.Type) {
            case ApiKey:
            case StaticToken:
                if (string.IsNullOrWhiteSpace(Authentication.Value))
                    result += new ValidationError("The http client authentication value is missing.", GetConfigurationPath(providerPath, $"{nameof(Authentication)}:{nameof(HttpClientAuthentication.Value)}")); ;
                break;
            default:
                result += new ValidationError("The http client authentication type is invalid.", GetConfigurationPath(providerPath, nameof(AuthenticationType)));
                break;
        }
        return result;
    }

    private static string GetConfigurationPath(string providerPath, string key)
        => $"Configuration[{SectionName}{providerPath}:{key}]";

    public void Configure(HttpClient client) {
        client.BaseAddress = new(BaseAddress);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(GetDefaultIfNullOrWhiteSpace(ResponseFormat, DefaultResponseFormat)));
        foreach (var customHeader in CustomHeaders ?? [])
            client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
        if (Authentication is null) return;
        client.DefaultRequestHeaders.Authorization = Authentication;
    }
}
