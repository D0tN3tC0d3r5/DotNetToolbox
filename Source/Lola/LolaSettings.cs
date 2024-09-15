namespace Lola;

public class LolaSettings
    : ApplicationSettings {
    public string DefaultAIProvider { get; set; } = string.Empty;
    public string[] AvailableModels { get; set; } = [];
    public string CurrentAgentKey { get; set; } = string.Empty;
}
