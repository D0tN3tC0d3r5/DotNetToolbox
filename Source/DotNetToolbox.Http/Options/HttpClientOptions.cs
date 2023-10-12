namespace DotNetToolbox.Http.Options;

public record HttpClientOptions  {
    public const string DefaultResponseFormat = "application/json";

    public HttpClientOptions() {
    }

    [SetsRequiredMembers]
    internal HttpClientOptions(IConfiguration configuration, HttpClientOptions? parent = null) {
        Configure(configuration, parent);
    }

    public string BaseAddress { get; set; } = default!;

    public string ResponseFormat { get; set; } = DefaultResponseFormat;

    public Dictionary<string, string[]> CustomHeaders { get; set; } = new();

    public AuthenticationOptions? Authentication { get; set; }

    public virtual ValidationResult Validate() {
        var result = Success();

        if (string.IsNullOrWhiteSpace(BaseAddress))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(BaseAddress));

        if (string.IsNullOrWhiteSpace(ResponseFormat))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(ResponseFormat));

        result += Authentication?.Validate() ?? Success();

        return result;
    }

    internal void Configure(IConfiguration config, HttpClientOptions? parent) {
        BaseAddress = config.GetValue<string>(nameof(BaseAddress)) ?? parent?.BaseAddress ?? string.Empty;
        ResponseFormat = config.GetValue<string>(nameof(ResponseFormat)) ?? parent?.ResponseFormat ?? DefaultResponseFormat;
        var namedCustomHeaders = config.GetValue<Dictionary<string, string[]>>(nameof(CustomHeaders)) ?? new();
        var parentCustomHeaders = parent?.CustomHeaders ?? new();
        CustomHeaders = namedCustomHeaders.UnionBy(parentCustomHeaders.ExceptBy(namedCustomHeaders.Keys, i => i.Key), i => i.Key).ToDictionary();
        var authConfig = config.GetSection(nameof(AuthenticationOptions));
        if (authConfig.Value is null) {
            Authentication = parent?.Authentication;
            return;
        }

        var authType = authConfig.GetValue<AuthenticationType>("Type");
        Authentication = authType switch {
            None => null,
            ApiKey => new ApiKeyAuthenticationOptions(authConfig),
            StaticToken => new StaticTokenAuthenticationOptions(authConfig),
            Jwt => new JwtAuthenticationOptions(authConfig),
            OAuth2 => new OAuth2TokenAuthenticationOptions(authConfig),
            _ => throw new InvalidOperationException($"Authentication type '{authType}' not supported."),
        };
    }

    internal void Configure(HttpClient client, ref HttpAuthentication authentication) {
        client.BaseAddress = new(BaseAddress);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(ResponseFormat));
        foreach ((var key, var value) in CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        Authentication?.Configure(client, ref authentication);
    }
}