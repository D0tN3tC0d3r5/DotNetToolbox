namespace Sophia.Models.Providers;

public class ApiData {
    public string BaseAddress { get; set; } = string.Empty;
    public string ChatEndpoint { get; set; } = string.Empty;
    public Dictionary<string, string> CustomRequestHeaders { get; set; } = [];
}
