namespace Sophia.Models.Providers;

public class ApiData {
    public string BaseAddress { get; init; } = string.Empty;
    public string ChatEndpoint { get; init; } = string.Empty;
    public AuthorizationData Authorization { get; init; } = new();
    public Dictionary<string, string[]>? CustomRequestHeaders { get; set; }
}
