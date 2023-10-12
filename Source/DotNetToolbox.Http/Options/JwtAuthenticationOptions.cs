namespace DotNetToolbox.Http.Options;

public record JwtAuthenticationOptions : AuthenticationOptions {

    public JwtAuthenticationOptions() {
    }

    [SetsRequiredMembers]
    public JwtAuthenticationOptions(IConfiguration config)
        : this() {
        ExpiresAfter = config.GetValue<TimeSpan?>(nameof(ExpiresAfter));
        Audience = config.GetValue<string>(nameof(Audience));
        Issuer = config.GetValue<string>(nameof(Issuer));
        Claims = config.GetValue<Claim[]>(nameof(Claims)) ?? Array.Empty<Claim>();
        PrivateKey = IsNotNullOrWhiteSpace(config.GetValue<string>(nameof(PrivateKey)));
    }

    public string? PrivateKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public IReadOnlyList<Claim> Claims { get; set; } = Array.Empty<Claim>();
    public TimeSpan? ExpiresAfter { get; set; }

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(PrivateKey))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(PrivateKey)));

        return result;
    }

    internal override void Configure(HttpClient client, ref HttpClientAuthentication authentication) {
        if (!authentication.IsValid(Jwt)) authentication = CreateJwtToken();
        client.DefaultRequestHeaders.Authorization = authentication;
    }

    private HttpClientAuthentication CreateJwtToken() {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PrivateKey!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow + ExpiresAfter;
        var tokenOptions = new JwtSecurityToken(Issuer,
                                                Audience,
                                                Claims,
                                                null,
                                                expiration,
                                                signingCredentials);

        return new() {
            Type = Jwt,
            Value = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
            Scheme = Bearer,
            ExpiresOn = expiration,
        };
    }
}