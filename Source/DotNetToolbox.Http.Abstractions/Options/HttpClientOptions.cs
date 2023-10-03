namespace DotNetToolbox.Http.Options;

public record HttpClientOptions {
    [Required]
    public required string BaseAddress { get; init; }

    public string ResponseFormat { get; init; } = "application/json";

    public Dictionary<string, string> CustomHeaders { get; init; } = new();

    public string? ApiKey { get; init; }

    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Authority { get; init; }
    public string[] Scopes { get; init; } = Array.Empty<string>();
}
