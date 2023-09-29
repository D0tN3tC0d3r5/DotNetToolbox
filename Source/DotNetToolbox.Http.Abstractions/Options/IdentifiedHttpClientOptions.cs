namespace DotNetToolbox.Http.Options;

public record IdentifiedHttpClientOptions : HttpClientOptions
{
    public required string ApiKey { get; init; }
}
