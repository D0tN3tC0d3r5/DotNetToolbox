namespace DotNetToolbox.Http.Options;

public record HttpClientOptions : SimpleHttpClientOptions {
    public Dictionary<string, SimpleHttpClientOptions> Clients { get; set; } = new();

    internal void Configure(IConfiguration config) {
        base.Configure(config, null);

        var clientsSection = config.GetSection(nameof(Clients));
        if (clientsSection.Value is null)
            return;

        var clientSections = clientsSection.GetChildren().ToArray();
        if (clientSections.Length == 0)
            return;

        foreach (var clientConfig in clientSections)
            Clients.Add(clientConfig.Key, new(clientConfig.GetSection(clientConfig.Key), this));
    }

    public override ValidationResult Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate(context);

        foreach (var client in Clients)
            result += client.Value.Validate(client.Key);

        return result;
    }

    public SimpleHttpClientOptions ResolveOptionsFor(string? clientName = null) {
        var options = (SimpleHttpClientOptions)this;
        return clientName is null || Clients.TryGetValue(clientName, out options)
            ? options
            : throw new ArgumentException($"Http client options for '{clientName}' not found.", nameof(clientName));
    }
}
