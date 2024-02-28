namespace DotNetToolbox.OpenAI.Agents;

public class AgentBuilder() {
    private AgentOptions _options = new();

    public string SystemMessage { get; init; } = "You are a helpful agent.";

    public AgentBuilder Use(string model) {
        _options = _options with { Model = IsNotNull(model) };
        return this;
    }

    public AgentBuilder WithFrequencyPenalty(decimal frequencyPenalty) {
        _options = _options with { FrequencyPenalty = frequencyPenalty };
        return this;
    }

    public AgentBuilder WithPresencePenalty(decimal presencePenalty) {
        _options = _options with { PresencePenalty = presencePenalty };
        return this;
    }

    public AgentBuilder WithMaximumTokensPerMessage(uint maximumTokensPerMessage) {
        _options = _options with { MaximumTokensPerMessage = maximumTokensPerMessage };
        return this;
    }

    public AgentBuilder WithNumberOfChoices(byte numberOfChoices) {
        _options = _options with { NumberOfChoices = numberOfChoices };
        return this;
    }

    public AgentBuilder AddStopSignal(string stopSignal) {
        _options = _options with { StopSignals = [.. _options.StopSignals, stopSignal] };
        return this;
    }

    public AgentBuilder WithTemperature(decimal temperature) {
        _options = _options with { Temperature = temperature };
        return this;
    }

    public AgentBuilder WithTopProbability(decimal topProbability) {
        _options = _options with { TopProbability = topProbability };
        return this;
    }

    public AgentBuilder AddTool(Function function) {
        _options = _options with {
            Tools = [.. _options.Tools,
                new() {
                    Function = function,
                }],
        };
        return this;
    }

    public AgentOptions Build() => IsValid(_options);
}
