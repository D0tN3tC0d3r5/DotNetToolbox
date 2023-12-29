namespace DotNetToolbox.OpenAI.Chats;

public record ChatOptions : IValidatable {
    public const string DefaultChatModel = "gpt-3.5-turbo-1106";
    public const byte DefaultFrequencyPenalty = 0;
    public const sbyte MinimumFrequencyPenalty = -2;
    public const byte MaximumFrequencyPenalty = 2;
    public const byte DefaultPresencePenalty = 0;
    public const sbyte MinimumPresencePenalty = -2;
    public const byte MaximumPresencePenalty = 2;
    public const uint DefaultTokensPerMessage = 8192;
    public const uint MinimumTokensPerMessage = 1024;
    public const byte DefaultNumberOfChoices = 1;
    public const byte MinimumNumberOfChoices = 1;
    public const byte MaximumNumberOfChoices = 5;
    public const byte MaximumNumberOfStopSignals = 4;
    public const byte DefaultTemperature = 1;
    public const byte MinimumTemperature = 0;
    public const byte MaximumTemperature = 2;
    public const byte DefaultTopProbability = 1;
    public const byte MinimumTopProbability = 0;
    public const byte MaximumTopProbability = 1;

    public ChatOptions(string? model = null) {
        Model = model ?? Model;
    }

    public string Model { get; } = DefaultChatModel;
    public virtual uint MaximumTokensPerMessage { get; init; } = DefaultTokensPerMessage;
    public virtual byte NumberOfChoices { get; init; } = DefaultNumberOfChoices;
    public virtual decimal FrequencyPenalty { get; init; } = DefaultFrequencyPenalty;
    public virtual decimal PresencePenalty { get; init; } = DefaultPresencePenalty;
    public virtual HashSet<string> StopSignals { get; init; } = [];
    public virtual decimal Temperature { get; init; } = DefaultTemperature;
    public virtual decimal TopProbability { get; init; } = DefaultTopProbability;
    public virtual HashSet<Tool> Tools { get; init; } = [];
    public virtual bool UseStreaming { get; init; }

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (MaximumTokensPerMessage < MinimumTokensPerMessage)
            result += new ValidationError(nameof(MaximumTokensPerMessage), "Value must be greater than {0}. Found: {1}", MinimumTokensPerMessage, MaximumTokensPerMessage);

        if (NumberOfChoices is < MinimumNumberOfChoices or > MaximumNumberOfChoices)
            result += new ValidationError(nameof(NumberOfChoices), "Value must be between {0} and {1}. Found: {2}", MinimumNumberOfChoices, MaximumNumberOfChoices, NumberOfChoices);

        if (FrequencyPenalty is < MinimumFrequencyPenalty or > MaximumFrequencyPenalty)
            result += new ValidationError(nameof(FrequencyPenalty), "Value must be between {0} and {1}. Found: {2}", MinimumFrequencyPenalty, MaximumFrequencyPenalty, FrequencyPenalty);

        if (PresencePenalty is < MinimumPresencePenalty or > MaximumPresencePenalty)
            result += new ValidationError(nameof(PresencePenalty), "Value must be between {0} and {1}. Found: {2}", MinimumPresencePenalty, MaximumPresencePenalty, PresencePenalty);

        if (StopSignals.Count > MaximumNumberOfStopSignals)
            result += new ValidationError(nameof(StopSignals), "The maximum number of stop signals is {0}. Found: {1}.", MaximumNumberOfStopSignals, StopSignals.Count);

        if (StopSignals.Count > 0 && StopSignals.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError(nameof(StopSignals), "Stop signals cannot be null, empty, or contain only whitespace.");

        if (Temperature is < MinimumTemperature or > MaximumTemperature)
            result += new ValidationError(nameof(Temperature), "Value must be between {0} and {1}. Found: {2}", MinimumTemperature, MaximumTemperature, Temperature);

        if (TopProbability is < MinimumTopProbability or > MaximumTopProbability)
            result += new ValidationError(nameof(TopProbability), "Value must be between {0} and {1}. Found: {2}", MinimumTopProbability, MaximumTopProbability, TopProbability);

        return result;
    }
}
