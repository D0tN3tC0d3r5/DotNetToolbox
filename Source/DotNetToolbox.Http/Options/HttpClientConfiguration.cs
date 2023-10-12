namespace DotNetToolbox.Http.Options;

public record HttpClientConfiguration : HttpClientOptions {
    public Dictionary<string, HttpClientOptions> Clients { get; set; } = new();

    internal void Read(IConfiguration config) {
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

    public override ValidationResult Validate()
        => Clients.Aggregate(base.Validate(), (current, client) => current + client.Value.Validate());

    public HttpClientOptions ResolveOptionsFor(string? clientName = null) {
        var options = (HttpClientOptions)this;
        return clientName is null || Clients.TryGetValue(clientName, out options)
            ? options
            : throw new ArgumentException($"Http client options for '{clientName}' not found.", nameof(clientName));
    }
}
