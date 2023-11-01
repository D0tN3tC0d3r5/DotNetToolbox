namespace DotNetToolbox.Http.Options;

public class HttpClientConfiguration : HttpClientOptions {
    public Dictionary<string, HttpClientOptions> Clients { get; set; } = [];

    public HttpClientOptions ResolveOptionsFor(string? clientName = null) {
        var options = (HttpClientOptions)this;
        return clientName is null || Clients.TryGetValue(clientName, out options)
            ? options
            : throw new ArgumentException($"Http client options for '{clientName}' not found.", nameof(clientName));
    }
}
