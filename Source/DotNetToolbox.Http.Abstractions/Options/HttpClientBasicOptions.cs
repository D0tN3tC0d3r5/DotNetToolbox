using System.Results;

namespace DotNetToolbox.Http.Options;

public class HttpClientBasicOptions {
    public string? BaseAddress { get; set; }

    public string? ResponseFormat { get; set; }

    public HttpClientAuthorizationOptions? Authorization { get; set; }

    public Dictionary<string, string[]> CustomHeaders { get; set; } = new();
}