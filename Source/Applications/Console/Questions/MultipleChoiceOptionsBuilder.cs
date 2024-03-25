namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoiceOptionsBuilder<TValue> {
    private readonly List<MultipleChoiceOption<TValue>> _options = [];

    public virtual MultipleChoiceOptionsBuilder<TValue> AddOption(TValue value, string text, string? alias = null) {
        _options.Add(new (_options.Count, value, text, alias));
        return this;
    }

    public virtual MultipleChoiceOptionsBuilder<TValue> AddOption(TValue value, string text, bool displayIndex) {
        _options.Add(new(_options.Count, value, text, displayIndex));
        return this;
    }

    public virtual IEnumerable<MultipleChoiceOption<TValue>> Build()
        => _options;
}
