namespace DotNetToolbox.Http.Options;

public class JwtAuthenticationOptions : AuthenticationOptions {

    public string? PrivateKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public IReadOnlyList<Claim> Claims { get; set; } = Array.Empty<Claim>();
    public TimeSpan? ExpiresAfter { get; set; }

    public override Result Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate(context);

        if (string.IsNullOrWhiteSpace(PrivateKey))
            result += new ValidationError(ValueCannotBeNullOrWhiteSpace, GetSourcePath(nameof(PrivateKey)));

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }

    internal override HttpAuthentication Configure(HttpClient client, HttpAuthentication authentication) {
        if (!authentication.IsValid(Jwt)) authentication = CreateJwtToken();
        client.DefaultRequestHeaders.Authorization = authentication;
        return authentication;
    }

    internal DateTimeProvider DateTimeProvider { get; set; } = new();

    private HttpAuthentication CreateJwtToken() {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PrivateKey!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var now = DateTimeProvider.UtcNow;
        var expiration = now.DateTime + ExpiresAfter;
        var tokenOptions = new JwtSecurityToken(Issuer,
                                                Audience,
                                                Claims,
                                                now.DateTime,
                                                expiration,
                                                signingCredentials);

        return new() {
            DateTimeProvider = DateTimeProvider,
            Type = Jwt,
            Value = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
            Scheme = Bearer,
            ExpiresOn = expiration,
        };
    }
}
