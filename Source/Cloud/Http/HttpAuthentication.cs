namespace DotNetToolbox.Http;

public class HttpAuthentication {
    public AuthenticationType Type { get; init; } = None;
    public string Value { get; init; } = string.Empty;
    public AuthenticationScheme Scheme { get; init; } = Basic;
    public DateTime? ExpiresOn { get; init; }

    internal DateTimeProvider DateTimeProvider { get; set; } = new();

    public bool IsValid(AuthenticationType type)
        => !string.IsNullOrWhiteSpace(Value)
        && Type == type
        && (ExpiresOn is null || ExpiresOn.Value > DateTimeProvider.UtcNow);

    public static implicit operator AuthenticationHeaderValue(HttpAuthentication auth)
        => new(auth.Scheme.ToString(), auth.Value);
}
