namespace DotNetToolbox.Http.Options;

public record StaticTokenAuthenticationOptions : AuthenticationOptions {
    public StaticTokenAuthenticationOptions() {
    }

    [SetsRequiredMembers]
    public StaticTokenAuthenticationOptions(IConfiguration config)
        : this() {
        Scheme = config.GetValue<AuthenticationScheme?>(nameof(Scheme)) ?? Basic;
        Token = IsNotNullOrWhiteSpace(config.GetValue<string>(nameof(Token)));
    }

    public AuthenticationScheme Scheme { get; set; } = Basic;
    public string Token { get; set; } = string.Empty;

    internal override ValidationResult Validate() {
        var result = base.Validate();

        if (string.IsNullOrWhiteSpace(Token))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(Token));

        return result;
    }

    internal override void Configure(HttpClient client, ref HttpAuthentication authentication) {
        authentication = new() {
            Type = Jwt,
            Value = Token,
        };
        client.DefaultRequestHeaders.Authorization = authentication;
    }
}