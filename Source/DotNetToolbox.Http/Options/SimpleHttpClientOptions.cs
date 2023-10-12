namespace DotNetToolbox.Http.Options;

public record SimpleHttpClientOptions : IValidatable {
    public const string DefaultResponseFormat = "application/json";
    private const string _clientNameContextKey = "ClientName";

    public SimpleHttpClientOptions() {
    }

    [SetsRequiredMembers]
    internal SimpleHttpClientOptions(IConfiguration config, SimpleHttpClientOptions? parent = null) {
        Configure(config: config, parent: parent);
    }

    internal void Configure(IConfiguration config, SimpleHttpClientOptions? parent) {
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
            ApiKey => new ApiKeyAuthenticationOptions(authConfig),
            StaticToken => new StaticTokenAuthenticationOptions(authConfig),
            Jwt => new JwtAuthenticationOptions(authConfig),
            OAuth2 => new OAuth2TokenAuthenticationOptions(authConfig),
            _ => throw new InvalidOperationException($"Authentication type '{authType}' not supported."),
        };
    }

    public string BaseAddress { get; set; } = default!;

    public string ResponseFormat { get; set; } = DefaultResponseFormat;

    public Dictionary<string, string[]> CustomHeaders { get; set; } = new();

    public AuthenticationOptions? Authentication { get; set; }

    public virtual ValidationResult Validate(IDictionary<string, object?>? context = null) {
        var result = Success();

        var httpClientName = GetClientNameOrDefault();

        if (string.IsNullOrWhiteSpace(BaseAddress))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(BaseAddress)));

        if (string.IsNullOrWhiteSpace(ResponseFormat))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(ResponseFormat)));

        result += Authentication?.Validate(httpClientName) ?? Success();

        return result;

        string? GetClientNameOrDefault()
            => context is not null
            && context.TryGetValue(_clientNameContextKey, out var contextValue)
            ? contextValue as string
            : null;

        static string GetSource(string? name, params string[] fields)
            => $"{(name is null ? string.Empty : $"{name}.")}"
             + $"{string.Join(".", fields)}";
    }

    public virtual ValidationResult Validate(string? httpClientName = null) {
        var context = new Dictionary<string, object?>();
        if (httpClientName is not null) context[_clientNameContextKey] = httpClientName;
        return Validate(context);
    }

    internal void Configure(HttpClient client, ref HttpClientAuthentication authentication) {
        client.BaseAddress = new(BaseAddress);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(ResponseFormat));
        foreach ((var key, var value) in CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        Authentication?.Configure(client, ref authentication);
    }
}