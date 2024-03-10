﻿namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatOptions
    : IChatOptions {
    public const string DefaultApiEndpoint = "v1/chat/completions";
    public const string DefaultChatModel = "gpt-3.5-turbo-0125";
    public const byte DefaultFrequencyPenalty = 0;
    public const sbyte MinimumFrequencyPenalty = -2;
    public const byte MaximumFrequencyPenalty = 2;
    public const byte DefaultPresencePenalty = 0;
    public const sbyte MinimumPresencePenalty = -2;
    public const byte MaximumPresencePenalty = 2;
    public const uint DefaultMaximumOutputTokens = 4096;
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

    public string ApiEndpoint { get; set; } = DefaultApiEndpoint;
    public string Model { get; set; } = DefaultChatModel;
    public uint MaximumOutputTokens { get; set; } = DefaultMaximumOutputTokens;
    public decimal? Temperature { get; set; }
    public decimal? MinimumTokenProbability { get; set; }
    public HashSet<string> StopSequences { get; set; } = [];
    public bool UseStreaming { get; set; }

    public string? AgentName { get; set; }
    public byte? NumberOfChoices { get; set; }
    public decimal? FrequencyPenalty { get; set; }
    public decimal? PresencePenalty { get; set; }
    public HashSet<OpenAIChatTool> Tools { get; set; } = [];

    public Result Validate(IDictionary<string, object?>? context = null) {
        var result = Result.Success();
        if (NumberOfChoices is < MinimumNumberOfChoices or > MaximumNumberOfChoices)
            result += new ValidationError($"Value must be between {MinimumNumberOfChoices} and {MaximumNumberOfChoices}. Found: {NumberOfChoices}", nameof(NumberOfChoices));

        if (FrequencyPenalty is < MinimumFrequencyPenalty or > MaximumFrequencyPenalty)
            result += new ValidationError($"Value must be between {MinimumFrequencyPenalty} and {MaximumFrequencyPenalty}. Found: {FrequencyPenalty}", nameof(FrequencyPenalty));

        if (PresencePenalty is < MinimumPresencePenalty or > MaximumPresencePenalty)
            result += new ValidationError($"Value must be between {MinimumPresencePenalty} and {MaximumPresencePenalty}. Found: {PresencePenalty}", nameof(PresencePenalty));

        if (StopSequences.Count > MaximumNumberOfStopSignals)
            result += new ValidationError($"The maximum number of stop signals is {MaximumNumberOfStopSignals}. Found: {StopSequences.Count}.", nameof(StopSequences));

        if (StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace))
            result += new ValidationError("Stop signals cannot be null, empty, or contain only whitespace.", nameof(StopSequences));

        if (Temperature is < MinimumTemperature or > MaximumTemperature)
            result += new ValidationError($"Value must be between {MinimumTemperature} and {MinimumTemperature}. Found: {Temperature}", nameof(Temperature));

        if (MinimumTokenProbability is < MinimumTopProbability or > MaximumTopProbability)
            result += new ValidationError($"Value must be between {MinimumTopProbability} and {MaximumTopProbability}. Found: {MinimumTokenProbability}", nameof(MinimumTokenProbability));

        return result;
    }
}
