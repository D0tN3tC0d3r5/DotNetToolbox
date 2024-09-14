namespace DotNetToolbox.Environment;

public class TextPromptBuilder<TValue>(string prompt, IOutput output)
    : ITextPromptBuilder<TValue> {
    private readonly List<TValue> _choices = [];
    private string _prompt = prompt;
    private bool _isRequired;
    private bool _addLineBreak;
    private Func<TValue, CancellationToken, Task<Result>>? _validator;
    private Func<TValue, string>? _converter;
    private char? _maskChar;
    private TValue? _defaultChoice;

    [MemberNotNullWhen(true, nameof(_defaultChoice))]
    private bool HasDefault { get; set; }

    public TextPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter) {
        _converter = converter;
        return this;
    }

    public TextPromptBuilder<TValue> WithDefault(TValue defaultValue) {
        if (defaultValue is null) return this;
        _defaultChoice = defaultValue;
        HasDefault = _defaultChoice is not null
                  && (_defaultChoice is not string text || !string.IsNullOrEmpty(text));
        return this;
    }

    public TextPromptBuilder<TValue> AnswerOnANewLine() {
        _addLineBreak = true;
        return this;
    }

    public TextPromptBuilder<TValue> UseMask(char? maskChar) {
        _maskChar = maskChar ?? '*';
        return this;
    }

    public TextPromptBuilder<TValue> Validate(Func<TValue, Result> validate)
        => Validate((value, ct) => Task.FromResult(validate(value)));

    public TextPromptBuilder<TValue> Validate(Func<TValue, CancellationToken, Task<Result>> validate) {
        var oldValidator = _validator;
        _validator = async (value, ct) => {
            var result = oldValidator is null ? Result.Success() : await oldValidator(value, ct);
            result += await validate(value, ct);
            return result;
        };
        return this;
    }

    public TextPromptBuilder<TValue> AddChoices(TValue choice, params TValue[] otherChoices) {
        _choices.AddRange([choice, ..otherChoices]);
        return this;
    }

    private Func<TValue, ValidationResult> BuildValidator()
        => value => {
            var result = _validator?.Invoke(value, CancellationToken.None).GetAwaiter().GetResult() ?? Result.Success();
            if (result.IsSuccess) return ValidationResult.Success();
            if (result.Errors.Count == 1) return ValidationResult.Error($"[red]{result.Errors[0].Message}[/]");
            var errors = new StringBuilder();
            errors.AppendLine($"[red]The entry is invalid.[/]");
            foreach (var item in result.Errors)
                errors.AppendLine($"[red] - {item.Message}[/]");
            return ValidationResult.Error(errors.ToString());
        };

    public TValue Show() => ShowAsync().GetAwaiter().GetResult();

    public Task<TValue> ShowAsync(CancellationToken ct = default) {
        _prompt = $"[teal]{_prompt}[/]";
        if (!_isRequired) _prompt = $"[green][[Optional]][/] {_prompt}";
        if (_addLineBreak) _prompt += $"{output.NewLine}{output.Prompt}";
        var prompt = new TextPrompt<TValue>(_prompt);
        prompt.AllowEmpty().ChoicesStyle(new(foreground: Color.Blue));
        if (_maskChar is not null) prompt = prompt.Secret(_maskChar);
        if (HasDefault) prompt.DefaultValue(_defaultChoice);
        if (_choices.Count > 0) {
            prompt.AddChoices(_choices);
            prompt.ShowChoices();
            var oldValidator = _validator;
            _validator = async (value, ct) => {
                var result = oldValidator is null ? Result.Success() : await oldValidator(value, ct);
                if (!_choices.Contains(value)) result += new ValidationError($"The entry must be one of the given choices.");
                return result;
            };
        }
        if (_converter is not null) prompt.Converter = _converter;
        if (_validator is not null) prompt.Validator = BuildValidator();

        return prompt.ShowAsync(AnsiConsole.Console, ct);
    }
}
