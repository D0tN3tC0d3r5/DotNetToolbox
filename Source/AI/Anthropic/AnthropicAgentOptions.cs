namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgentOptions(string apiEndpoint, string model, string? name)
    : IAgentOptions {
    public AnthropicAgentOptions() : this(DefaultApiEndpoint, DefaultModel, null) {
    }

    public const string DefaultApiEndpoint = "v1/messages";
    public const string DefaultModel = "claude-2.1";
    public const byte MinimumTemperature = 0;
    public const byte MaximumTemperature = 2;
    public const byte MinimumTokenProbabilityCutOff = 0;
    public const byte MaximumTokenProbabilityCutOff = 1;

    public string? Name { get; set; } = name;
    public string ApiEndpoint { get; set; } = apiEndpoint;
    public string Model { get; set; } = model;
    public uint MaximumOutputTokens { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? TokenProbabilityCutOff { get; set; }
    public HashSet<string> StopSequences { get; set; } = [];
    public bool UseStreaming { get; set; }

    public uint? MaximumTokensToSample { get; set; }

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError("Stop signals cannot be null, empty, or contain only whitespace.", nameof(StopSequences));

        if (Temperature is < MinimumTemperature or > MaximumTemperature)
            result += new ValidationError($"Value must be between {MinimumTemperature} and {MinimumTemperature}. Found: {Temperature}", nameof(Temperature));

        if (TokenProbabilityCutOff is < MinimumTokenProbabilityCutOff or > MaximumTokenProbabilityCutOff)
            result += new ValidationError($"Value must be between {MinimumTokenProbabilityCutOff} and {MaximumTokenProbabilityCutOff}. Found: {TokenProbabilityCutOff}", nameof(TokenProbabilityCutOff));

        return result;
    }
}
