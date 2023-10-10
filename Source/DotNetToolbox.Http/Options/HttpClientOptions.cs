using System.Options;

namespace DotNetToolbox.Http.Options;

public class HttpClientOptions : SimpleHttpClientOptions, IValidatable {
    public Dictionary<string, SimpleHttpClientOptions> Clients { get; set; } = new();

    public IValidationResult Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate();

        foreach (var client in Clients)
            result += client.Value.Validate(client.Key);

        return result;
    }
}
