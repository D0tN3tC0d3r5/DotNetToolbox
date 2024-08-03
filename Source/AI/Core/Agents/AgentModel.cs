namespace DotNetToolbox.AI.Agents;

public class AgentModel
    : IAgentModel {
    private const byte _maximumRetries = 10;
    private const byte _maximumTemperature = 2;
    private const byte _maximumTokenProbabilityCutOff = 1;

    public const uint MinimumOutputTokens = 256;
    private const uint _defaultMaximumOutputTokens = MinimumOutputTokens;
    private const byte _defaultNumberOfRetries = 0;
    private const byte _defaultTemperature = 1;
    private const decimal _defaultProbabilityCutOff = 0;

    public required IModel Model { get; init; } = default!;
    public byte MaximumNumberOfRetries { get; init; } = _defaultNumberOfRetries;
    public uint MaximumOutputTokens { get; init; } = _defaultMaximumOutputTokens;
    public decimal Temperature { get; init; } = _defaultTemperature;
    public decimal TokenProbabilityCutOff { get; init; } = _defaultProbabilityCutOff;
    public List<string> StopSequences { get; init; } = [];
    public bool ResponseIsStream { get; set; }
    public bool RespondsAsJson { get; set; }

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError("StopWaiting signals cannot be null, empty, or contain only whitespace.", nameof(StopSequences));
        if (MaximumNumberOfRetries > _maximumRetries)
            result += new ValidationError($"The maximum number of retries is {_maximumRetries}. Found: {MaximumNumberOfRetries}", nameof(MaximumNumberOfRetries));
        if (Temperature is < 0 or > _maximumTemperature)
            result += new ValidationError($"Content must be between {0} and {_maximumTemperature}. Found: {Temperature}", nameof(Temperature));
        if (TokenProbabilityCutOff is < 0 or > _maximumTokenProbabilityCutOff)
            result += new ValidationError($"Content must be between {0} and {_maximumTokenProbabilityCutOff}. Found: {TokenProbabilityCutOff}", nameof(TokenProbabilityCutOff));

        return result;
    }
}
