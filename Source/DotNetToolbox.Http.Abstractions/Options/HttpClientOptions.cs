using System.Options;

namespace DotNetToolbox.Http.Options;

public class HttpClientOptions : HttpClientBasicOptions {
    public Dictionary<string, HttpClientBasicOptions> Named { get; set; } = new();
}