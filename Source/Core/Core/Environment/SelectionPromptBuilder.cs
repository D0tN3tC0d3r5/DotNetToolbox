namespace DotNetToolbox.Environment;

public class SelectionPromptBuilder<TValue>(string prompt, IOutput output)
    : ISelectionPromptBuilder<TValue>
    where TValue : notnull {
    private readonly IOutput _output = output;
    private string _prompt = prompt;
    private bool _allowNoChoice = false;
    private string _noChoiceText = string.Empty;
    private Func<TValue, string>? _converter;
    private List<TValue?> _choices = [];
    private TValue? _defaultValue;
    private string _fieldName = "value";

    [MemberNotNullWhen(true, nameof(_defaultValue))]
    private bool HasDefault { get; set; }

    public SelectionPromptBuilder<TValue> For(string fieldName) {
        _fieldName = fieldName;
        return this;
    }

    public SelectionPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter) {
        _converter = converter;
        return this;
    }

    public SelectionPromptBuilder<TValue> WithDefault(TValue defaultValue) {
        _defaultValue = IsNotNull(defaultValue);
        HasDefault = true;
        return this;
    }

    public SelectionPromptBuilder<TValue> AllowNoChoice(string text) {
        _allowNoChoice = true;
        _noChoiceText = text;
        return this;
    }

    public SelectionPromptBuilder<TValue> AddChoice(TValue choice) {
        _choices.Add(choice);
        return this;
    }

    public SelectionPromptBuilder<TValue> AddChoices(params TValue[] otherChoices) {
        if (otherChoices.Length > 0) _choices.AddRange(otherChoices);
        return this;
    }

    public TValue Show() {
        var prompt = new SelectionPrompt<TValue>();
        if (_converter is not null) prompt.UseConverter(_converter);
        if (_allowNoChoice) {
            prompt.AddChoice(default!);
            var oldConverter = prompt.Converter;
            prompt.UseConverter(value => value is null ? _noChoiceText : oldConverter?.Invoke(value) ?? string.Empty);
        }
        prompt.AddChoices(_choices);
        var defaultText = _defaultValue is not null ? prompt.Converter?.Invoke(_defaultValue) : null;
        _prompt = defaultText is not null ? $"{_prompt} [blue]({defaultText})[/]" : _prompt;
        prompt.Title(_prompt);
        var result = AnsiConsole.Prompt(prompt);
        var resultText = result is not null ? prompt.Converter?.Invoke(result) ?? string.Empty : string.Empty;
        _output.WriteLine($"{_prompt}: {resultText}");
        return result!;
    }

    public static implicit operator TValue(SelectionPromptBuilder<TValue> promptBuilder) => promptBuilder.Show();
}
