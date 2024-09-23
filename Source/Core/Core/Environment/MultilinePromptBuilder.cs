namespace DotNetToolbox.Environment;

public class MultilinePromptBuilder(string prompt, IOutput output)
    : IMultilinePromptBuilder {
    private string _prompt = prompt;
    private string _defaultValue = string.Empty;
    private bool _singleLine;
    private Func<string, Result>? _validator;

    public MultilinePromptBuilder(IOutput output)
        : this(string.Empty, output) {
    }

    public MultilinePromptBuilder WithDefault(string? defaultValue) {
        if (string.IsNullOrEmpty(defaultValue)) return this;
        _defaultValue = defaultValue;
        return this;
    }

    public MultilinePromptBuilder AddValidation(Func<string, Result> validate) {
        var oldValidator = _validator;
        _validator = value => {
            var result = oldValidator?.Invoke(value) ?? Result.Success();
            result += validate(value);
            return result;
        };
        return this;
    }

    public MultilinePromptBuilder AsSingleLine() {
        _singleLine = true;
        return this;
    }

    public string Show() => ShowAsync().GetAwaiter().GetResult();

    public async Task<string> ShowAsync(CancellationToken ct = default) {
        while (true) {
            _prompt = $"[teal]{_prompt}[/]";
            output.WriteLine(_prompt);
            output.WriteLine(_singleLine
                                 ? "[gray]Press ENTER to submit and ESCAPE to cancel.[/]"
                                 : "[gray]Press ENTER to insert a new line, CTRL+ENTER to submit, and ESCAPE to cancel.[/]");
            var editor = new LineEditor {
                MultiLine = !_singleLine,
                Prompt = new LineEditorPrompt("[yellow]>[/]", "[yellow]|[/]"),
                Text = _defaultValue,
                Validator = BuildValidator(),
            };
            if (!_singleLine) {
                editor.KeyBindings.Add<NewLineCommand>(ConsoleKey.Enter);
                editor.KeyBindings.Add<SubmitCommand>(ConsoleKey.Enter, ConsoleModifiers.Control);
            }

            var result = await editor.ReadLine(ct) ?? string.Empty;
            if (editor.Result != ExitState.Invalid) return result;
            output.WriteLine(editor.ErrorMessage);
            output.WriteLine("[yellow]Please try again.[/]");
            output.WriteLine();
        }
    }

    private Func<string, ValidationResult>? BuildValidator()
        => _validator is null ? null : value => {
            var result = _validator(value);
            if (result.IsSuccess) return ValidationResult.Success();
            if (result.Errors.Count == 1) return ValidationResult.Error($"[red]{result.Errors[0].Message}[/]");
            var errors = new StringBuilder();
            errors.AppendLine("[red]The input value is invalid.[/]");
            foreach (var item in result.Errors)
                errors.AppendLine($"[red] - {item.Message}[/]");
            return ValidationResult.Error(errors.ToString());
        };
}
