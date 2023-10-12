namespace DotNetToolbox.Http;

public record HttpAuthentication {
    public AuthenticationType Type { get; init; } = None;
    public string Value { get; init; } = string.Empty;
    public AuthenticationScheme Scheme { get; init; } = Basic;
    public DateTime? ExpiresOn { get; init; }

    public bool IsValid(AuthenticationType type) => (Type == type)
                                        && string.IsNullOrWhiteSpace(Value)
                                        && (ExpiresOn is null || (ExpiresOn > DateTime.UtcNow));

    public static implicit operator AuthenticationHeaderValue(HttpAuthentication auth)
        => new(auth.Scheme.ToString(), auth.Value);
}