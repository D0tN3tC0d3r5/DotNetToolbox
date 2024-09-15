namespace DotNetToolbox.Environment;

public class SelectionPromptBuilder<TValue>(string prompt, IOutput output)
    : ISelectionPromptBuilder<TValue>
    where TValue : notnull {
    private readonly List<TValue> _choices = [];
    private string _prompt = prompt;
    private Func<TValue, string>? _converter;
    private TValue? _defaultValue;
    private bool _showResult;

    [MemberNotNullWhen(true, nameof(_defaultValue))]
    private bool HasDefault { get; set; }

    public SelectionPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter) {
        _converter = converter;
        return this;
    }

    public SelectionPromptBuilder<TValue> ShowResult() {
        _showResult = true;
        return this;
    }

    public SelectionPromptBuilder<TValue> WithDefault(TValue defaultValue) {
        _defaultValue = IsNotNull(defaultValue);
        HasDefault = true;
        return this;
    }

    public SelectionPromptBuilder<TValue> AddChoices(IEnumerable<TValue> choices) {
        _choices.AddRange(choices);
        return this;
    }

    public SelectionPromptBuilder<TValue> AddChoices(TValue choice, params TValue[] otherChoices) {
        _choices.AddRange([choice, ..otherChoices]);
        return this;
    }

    public TValue Show() => ShowAsync().GetAwaiter().GetResult();

    public async Task<TValue> ShowAsync(CancellationToken ct = default) {
        var prompt = new SelectionPrompt<TValue>();
        if (_converter is not null) prompt.UseConverter(_converter);
        prompt.AddChoices(_choices);
        var defaultText = _defaultValue is not null ? prompt.Converter?.Invoke(_defaultValue) : null;
        var isQuestion = _prompt.EndsWith('?');
        _prompt = HasDefault ? $"{_prompt} [blue]({defaultText})[/]" : _prompt;
        prompt.Title(_prompt);
        var result = await prompt.ShowAsync(AnsiConsole.Console, ct);
        var resultText = prompt.Converter?.Invoke(result) ?? string.Empty;
        var separator = isQuestion ? string.Empty : ":";
        if (_showResult) output.WriteLine($"{_prompt}{separator} [green]{resultText}[/]");
        return result;
    }
}
