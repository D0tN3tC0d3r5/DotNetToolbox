namespace AI.Sample;

public class LolaSettings()
    : ApplicationSettings {
    public string DefaultAIProvider { get; set; } = string.Empty;
    public string[] AvailableModels { get; set; } = [];
}
