namespace DotNetToolbox.Http.Options;

public record HttpClientOptions
{
    [Required]
    public required string BaseAddress { get; init; }

    public string ResponseFormat { get; init; } = "application/json";

    public Dictionary<string, string> CustomHeaders { get; init; } = new();
}
