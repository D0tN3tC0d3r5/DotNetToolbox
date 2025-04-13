namespace WebApi.Options;

public record TwoFactorAuthenticationOptions {
    public bool IsRequired { get; set; }
    public TwoFactorAuthenticationType Type { get; set; }
    public TemporaryTokenOptions Token { get; set; } = new();
}
