namespace DotNetToolbox.OpenAI.Chats;

public class ChatBuilder() {
    private ChatOptions _options = new();

    public string SystemMessage { get; init; } = "You are a helpful agent.";

    public ChatBuilder Use(string model) {
        _options = _options with { Model = IsNotNull(model) };
        return this;
    }

    public ChatBuilder WithFrequencyPenalty(decimal frequencyPenalty) {
        _options = _options with { FrequencyPenalty = frequencyPenalty };
        return this;
    }

    public ChatBuilder WithPresencePenalty(decimal presencePenalty) {
        _options = _options with { PresencePenalty = presencePenalty };
        return this;
    }

    public ChatBuilder WithMaximumTokensPerMessage(uint maximumTokensPerMessage) {
        _options = _options with { MaximumTokensPerMessage = maximumTokensPerMessage };
        return this;
    }

    public ChatBuilder WithNumberOfChoices(byte numberOfChoices) {
        _options = _options with { NumberOfChoices = numberOfChoices };
        return this;
    }

    public ChatBuilder AddStopSignal(string stopSignal) {
        _options = _options with { StopSignals = [.. _options.StopSignals, stopSignal] };
        return this;
    }

    public ChatBuilder WithTemperature(decimal temperature) {
        _options = _options with { Temperature = temperature };
        return this;
    }

    public ChatBuilder WithTopProbability(decimal topProbability) {
        _options = _options with { TopProbability = topProbability };
        return this;
    }

    public ChatBuilder AddTool(Function function) {
        _options = _options with { Tools = [.. _options.Tools, new(function) ] };
        return this;
    }

    public ChatOptions Build() => IsValid(_options);
}
