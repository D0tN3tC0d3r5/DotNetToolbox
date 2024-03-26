namespace Sophia.Models.Providers;

public class ApiData {
    public string BaseAddress { get; set; } = string.Empty;
    public string ChatEndpoint { get; set; } = string.Empty;
    public AuthorizationData Authorization { get; init; } = new();
    public Dictionary<string, string[]>? CustomRequestHeaders { get; set; }
}
