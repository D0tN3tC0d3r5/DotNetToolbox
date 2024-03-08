namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChatOptions
    : IChatOptions {
    public const string DefaultApiEndpoint = "v1/messages";
    public const string DefaultChatModel = "claude-2.1";
    public const string DefaultSystemMessage = "You are a helpful agent.";
    public const uint MinimumTokensPerMessage = 1024;
    public const byte MinimumTemperature = 0;
    public const byte MaximumTemperature = 2;
    public const byte MinimumTopProbability = 0;
    public const byte MaximumTopProbability = 1;

    public string ApiEndpoint { get; set; } = DefaultApiEndpoint;
    public string Model { get; set; } = DefaultChatModel;
    public string SystemMessage { get; set; } = DefaultSystemMessage;
    public uint MaximumTokensPerMessage { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? MinimumTokenProbability { get; set; }
    public HashSet<string> StopSequences { get; set; } = [];
    public bool UseStreaming { get; set; }

    public uint? MaximumTokensToSample { get; set; }

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (MaximumTokensPerMessage < MinimumTokensPerMessage)
            result += new ValidationError($"Value must be greater than {MinimumTokensPerMessage}. Found: {MaximumTokensPerMessage}", nameof(MaximumTokensPerMessage));

        if (StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError("Stop signals cannot be null, empty, or contain only whitespace.", nameof(StopSequences));

        if (Temperature is < MinimumTemperature or > MaximumTemperature)
            result += new ValidationError($"Value must be between {MinimumTemperature} and {MinimumTemperature}. Found: {Temperature}", nameof(Temperature));

        if (MinimumTokenProbability is < MinimumTopProbability or > MaximumTopProbability)
            result += new ValidationError($"Value must be between {MinimumTopProbability} and {MaximumTopProbability}. Found: {MinimumTokenProbability}", nameof(MinimumTokenProbability));

        return result;
    }
}
