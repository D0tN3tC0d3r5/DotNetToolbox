namespace DotNetToolbox.Environment;

public class TextPromptBuilder<TValue>(string prompt, IOutput output)
    : ITextPromptBuilder<TValue> {
    private readonly IOutput _output = output;
    private string _prompt = prompt;
    private bool _isRequired;
    private bool _addLineBreak;
    private Func<TValue, Result>? _validator;
    private Func<TValue, string>? _converter;
    private List<TValue> _choices = [];
    private char? _maskChar;
    private TValue? _defaultValue;
    private string _fieldName = "value";

    [MemberNotNullWhen(true, nameof(_defaultValue))]
    private bool HasDefault { get; set; }

    public TextPromptBuilder<TValue> For(string fieldName) {
        _fieldName = fieldName;
        return this;
    }

    public TextPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter) {
        _converter = converter;
        return this;
    }

    public TextPromptBuilder<TValue> WithDefault(TValue defaultValue) {
        if (defaultValue is null) return this;
        _defaultValue = defaultValue;
        HasDefault = _defaultValue is not null
                  && (_defaultValue is not string text || !string.IsNullOrEmpty(text));
        return this;
    }

    public TextPromptBuilder<TValue> AnswerInNewLine() {
        _addLineBreak = true;
        return this;
    }

    public TextPromptBuilder<TValue> AsRequired() {
        var oldValidator = _validator;
        _validator = value => {
            var result = oldValidator?.Invoke(value) ?? Result.Success();
            if (value is string text && string.IsNullOrEmpty(text)) result += Result.Invalid($"The {_fieldName} is required.");
            else if (value is null) result += Result.Invalid($"The {_fieldName} is required.");
            return result;
        };
        _isRequired = true;
        return this;
    }

    public TextPromptBuilder<TValue> UseMask(char? maskChar) {
        _maskChar = maskChar ?? '*';
        return this;
    }

    public TextPromptBuilder<TValue> Validate(Func<TValue, Result> validate) {
        var oldValidator = _validator;
        _validator = value => {
            var result = oldValidator?.Invoke(value) ?? Result.Success();
            result += validate(value);
            return result;
        };
        return this;
    }

    public TextPromptBuilder<TValue> Validate(Func<TValue, bool> validate, string errorMessage) {
        var oldValidator = _validator;
        _validator = value => {
            var result = oldValidator?.Invoke(value) ?? Result.Success();
            if (!validate(value)) result += Result.Invalid(errorMessage);
            return result;
        };
        return this;
    }

    public TextPromptBuilder<TValue> AddChoice(TValue choice) {
        _choices.Add(choice);
        return this;
    }

    public TextPromptBuilder<TValue> AddChoices(params TValue[] otherChoices) {
        if (otherChoices.Length > 0) _choices.AddRange(otherChoices);
        return this;
    }

    private Func<TValue, ValidationResult>? BuildValidator()
        => value => {
            var result = _validator?.Invoke(value);
            if (result?.IsSuccess != false) return ValidationResult.Success();
            if (result.Errors.Count == 1) return ValidationResult.Error($"[red]{result.Errors[0].Message}[/]");
            var errors = new StringBuilder();
            foreach (var item in result.Errors)
                errors.AppendLine($"[red] - {item.Message}[/]");
            return ValidationResult.Error(errors.ToString());
        };

    public TValue Show() {
        var isQuestion = _prompt.EndsWith('?');
        _prompt = $"[teal]{_prompt}[/]";
        if (!_isRequired) _prompt = $"[green][[Optional]][/] {_prompt}";
        //        if (HasDefault) _prompt += $" [[[blue]{_defaultValue}[/]]]";
        if (_addLineBreak) _prompt += _output.NewLine;
        var prompt = new TextPrompt<TValue>(_prompt);
        prompt.DefaultValue(_defaultValue);
        prompt.AllowEmpty()
              .DefaultValueStyle(new Style(foreground: Color.Green))
              .ChoicesStyle(new Style(foreground: Color.Blue));
        if (_maskChar is not null) prompt = prompt.Secret(_maskChar);
        if (_choices.Count > 0) {
            prompt.AddChoices(_choices);
            prompt.ShowChoices();
            var oldValidator = _validator;
            _validator = value => {
                var result = oldValidator?.Invoke(value) ?? Result.Success();
                if (!_choices.Contains(value)) result += new ValidationError($"The {_fieldName} must be one of the given choices.");
                return result;
            };
        }
        if (_converter is not null) prompt.Converter = _converter;
        if (_validator is not null) prompt.Validator = BuildValidator();

        return AnsiConsole.Prompt(prompt);
    }

    public static implicit operator TValue(TextPromptBuilder<TValue> promptBuilder) => promptBuilder.Show();
}
