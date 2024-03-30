namespace DotNetToolbox.AI.Agents;

public class AgentModel
    : IAgentModel {
    public const byte MaximumRetries = 10;
    public const byte MaximumTemperature = 2;
    public const byte MaximumTokenProbabilityCutOff = 1;

    public const uint DefaultMaximumOutputTokens = 256;
    public const byte DefaultNumberOfRetries = 0;
    public const byte DefaultTemperature = 1;
    public const decimal DefaultProbabilityCutOff = 0;

    public string ModelId { get; set; } = default!;
    public byte NumberOfRetries { get; set; } = DefaultNumberOfRetries;
    public uint MaximumOutputTokens { get; set; } = DefaultMaximumOutputTokens;
    public decimal Temperature { get; set; } = DefaultTemperature;
    public decimal TokenProbabilityCutOff { get; set; } = DefaultProbabilityCutOff;
    public List<string> StopSequences { get; set; } = [];
    public bool ResponseIsStream { get; set; } = false;
    public bool RespondsAsJson { get; set; } = false;

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError("StopWaiting signals cannot be null, empty, or contain only whitespace.", nameof(StopSequences));
        if (NumberOfRetries > MaximumRetries)
            result += new ValidationError($"The maximum number of retries is {MaximumRetries}. Found: {NumberOfRetries}", nameof(NumberOfRetries));
        if (Temperature is < 0 or > MaximumTemperature)
            result += new ValidationError($"Value must be between {0} and {MaximumTemperature}. Found: {Temperature}", nameof(Temperature));
        if (TokenProbabilityCutOff is < 0 or > MaximumTokenProbabilityCutOff)
            result += new ValidationError($"Value must be between {0} and {MaximumTokenProbabilityCutOff}. Found: {TokenProbabilityCutOff}", nameof(TokenProbabilityCutOff));

        return result;
    }
}
