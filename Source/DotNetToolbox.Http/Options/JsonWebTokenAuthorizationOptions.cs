namespace DotNetToolbox.Http.Options;

public class JsonWebTokenAuthorizationOptions : TokenAuthorizationOptions {
    public string? PrivateKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public IReadOnlyList<Claim> Claims { get; set; } = Array.Empty<Claim>();

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(PrivateKey))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(PrivateKey)));

        return result;
    }
}