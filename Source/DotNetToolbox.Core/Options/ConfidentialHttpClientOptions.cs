namespace DotNetToolbox.Options;

public record ConfidentialHttpClientOptions : HttpClientOptions
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string Authority { get; init; }
    public required string[]? Scopes { get; init; }
}
