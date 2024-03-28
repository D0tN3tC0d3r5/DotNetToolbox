namespace DotNetToolbox.AI.Agents;

public class AgentOptions
    : IAgentOptions {

    public const uint DefaultMaximumOutputTokens = 1024;
    public const byte DefaultNumberOfRetries = 0;
    public const byte DefaultTemperature = 1;
    public const decimal DefaultProbabilityCutOff = 0;

    public const byte MaximumRetries = 10;
    public const byte MaximumStopSequences = 4;
    public const byte MinimumTemperature = 0;
    public const byte MaximumTemperature = 2;
    public const byte MinimumTokenProbabilityCutOff = 0;
    public const byte MaximumTokenProbabilityCutOff = 1;

    //public string ChatEndpoint { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public byte NumberOfRetries { get; set; } = DefaultNumberOfRetries;
    public uint MaximumOutputTokens { get; set; } = DefaultMaximumOutputTokens;
    public decimal Temperature { get; set; } = DefaultTemperature;
    public decimal TokenProbabilityCutOff { get; set; } = DefaultProbabilityCutOff;
    public List<string> StopSequences { get; set; } = [];
    public bool UseStreaming { get; set; } = false;
    public bool JsonMode { get; set; } = false;

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError("StopWaiting signals cannot be null, empty, or contain only whitespace.", nameof(StopSequences));

        if (StopSequences.Count > MaximumStopSequences)
            result += new ValidationError($"The maximum number of stop sequences is {MaximumStopSequences}.. Found: {StopSequences.Count}", nameof(MaximumStopSequences));

        if (NumberOfRetries > MaximumRetries)
            result += new ValidationError($"The maximum number of retries is {MaximumRetries}. Found: {NumberOfRetries}", nameof(NumberOfRetries));

        if (Temperature is < MinimumTemperature or > MaximumTemperature)
            result += new ValidationError($"Value must be between {MinimumTemperature} and {MaximumTemperature}. Found: {Temperature}", nameof(Temperature));

        if (TokenProbabilityCutOff is < MinimumTokenProbabilityCutOff or > MaximumTokenProbabilityCutOff)
            result += new ValidationError($"Value must be between {MinimumTokenProbabilityCutOff} and {MaximumTokenProbabilityCutOff}. Found: {TokenProbabilityCutOff}", nameof(TokenProbabilityCutOff));

        return result;
    }
}
