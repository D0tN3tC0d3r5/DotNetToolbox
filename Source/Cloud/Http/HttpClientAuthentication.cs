namespace DotNetToolbox.Http;

public class HttpClientAuthentication() {
    private readonly IDateTimeProvider _dateTime = new DateTimeProvider();

    public HttpClientAuthentication(IDateTimeProvider dateTime)
        : this() {
        _dateTime = IsNotNull(dateTime);
    }

    public AuthenticationType Type { get; init; }
    public string? Value { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; }

    public bool IsExpired()
        => Type != None
        && ExpiresOn is not null
        && (string.IsNullOrWhiteSpace(Value) || _dateTime.UtcNow >= ExpiresOn.Value);

    public void Revoke() {
        if (IsExpired()) ExpiresOn = _dateTime.Minimum;
    }

    public void Authorize(string value, DateTimeOffset? expiresOn = null) {
        Value = IsNotNull(value);
        ExpiresOn = null;
        if (expiresOn is not null) ExtendUntil(expiresOn.Value);
    }

    public void ExtendUntil(DateTimeOffset expiresOn)
        => ExpiresOn = IsValid(expiresOn, d => d > _dateTime.UtcNow);

    public static implicit operator AuthenticationHeaderValue?(HttpClientAuthentication auth)
        => auth.Value is null
               ? default
               : auth.Type switch {
                   BasicToken => new("Basic", auth.Value),
                   BearerToken or Jwt or OAuth2 => new("Bearer", auth.Value),
                   Password => new("Basic", ToBase64String(UTF8.GetBytes(auth.Value))), // auth.Value should be in the form of "username:password"
                   _ => default,
               };

    public void SetHttpClientAuthentication(HttpClient client) {
        switch (Type) {
            case None: break;
            case ApiKey:
                client.DefaultRequestHeaders.Add("X-Api-Key", Value);
                break;
            case BasicToken:
            case BearerToken:
            case Jwt:
                client.DefaultRequestHeaders.Authorization = this;
                break;
            case Password: break;
            case OAuth2: break;
            default: break;
        };
    }
}
