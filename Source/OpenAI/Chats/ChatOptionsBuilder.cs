namespace DotNetToolbox.OpenAI.Chats;

public class ChatOptionsBuilder(string? model = null) {
    private ChatOptions _options = new(model);

    public ChatOptionsBuilder WithFrequencyPenalty(decimal frequencyPenalty) {
        _options = _options with { FrequencyPenalty = frequencyPenalty };
        return this;
    }

    public ChatOptionsBuilder WithPresencePenalty(decimal presencePenalty) {
        _options = _options with { PresencePenalty = presencePenalty };
        return this;
    }

    public ChatOptionsBuilder WithMaximumTokensPerMessage(uint maximumTokensPerMessage) {
        _options = _options with { MaximumTokensPerMessage = maximumTokensPerMessage };
        return this;
    }

    public ChatOptionsBuilder WithNumberOfChoices(byte numberOfChoices) {
        _options = _options with { NumberOfChoices = numberOfChoices };
        return this;
    }

    public ChatOptionsBuilder AddStopSignal(string stopSignal) {
        _options = _options with { StopSignals = [.. _options.StopSignals, stopSignal] };
        return this;
    }

    public ChatOptionsBuilder WithTemperature(decimal temperature) {
        _options = _options with { Temperature = temperature };
        return this;
    }

    public ChatOptionsBuilder WithTopProbability(decimal topProbability) {
        _options = _options with { TopProbability = topProbability };
        return this;
    }

    public ChatOptionsBuilder AddTool(Function function) {
        _options = _options with {
            Tools = [.. _options.Tools,
                new() {
                    Function = function,
                }]
        };
        return this;
    }

    public ChatOptionsBuilder EnableStreaming() {
        _options = _options with { UseStreaming = true };
        return this;
    }

    public ChatOptions Build() => IsValid(_options);
}
