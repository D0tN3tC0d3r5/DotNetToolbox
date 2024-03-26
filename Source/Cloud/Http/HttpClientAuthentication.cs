namespace DotNetToolbox.Http;

public class HttpClientAuthentication() {
    private readonly IDateTimeProvider _dateTime = new DateTimeProvider();

    public HttpClientAuthentication(IDateTimeProvider dateTime)
        : this() {
        _dateTime = IsNotNull(dateTime);
    }

    public AuthenticationType Type { get; init; }
    public AuthorizationScheme Scheme { get; init; }
    public string? Value { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; }

    public bool IsValid()
        => Type == None
        || ExpiresOn is null
        || (!string.IsNullOrWhiteSpace(Value) && ExpiresOn.Value > _dateTime.UtcNow);

    public void Revoke() {
        if (IsValid()) ExpiresOn = _dateTime.Minimum;
    }

    public void Authorize(string value, DateTimeOffset? expiresOn = null) {
        Value = IsNotNull(value);
        ExpiresOn = null;
        if (expiresOn is not null) ExtendUntil(expiresOn.Value);
    }

    public void ExtendUntil(DateTimeOffset expiresOn)
        => ExpiresOn = Ensure.IsValid(expiresOn, d => d > _dateTime.UtcNow);

    public static implicit operator AuthenticationHeaderValue(HttpClientAuthentication auth)
        => new(auth.Scheme.ToString(), auth.Value);
}
