namespace DotNetToolbox.Environment;

public class MultilinePromptBuilder(string prompt, IOutput output)
    : IMultilinePromptBuilder {
    private string _prompt = prompt;
    private Func<string, CancellationToken, Task<Result>>? _validator;

    public MultilinePromptBuilder(IOutput output)
        : this(string.Empty, output) {
    }

    public MultilinePromptBuilder Validate(Func<string, Result> validate)
        => Validate((value, ct) => Task.FromResult(validate(value)));

    public MultilinePromptBuilder Validate(Func<string, CancellationToken, Task<Result>> validate) {
        var oldValidator = _validator;
        _validator = async (value, ct) => {
            var result = oldValidator is null ? Result.Success() : await oldValidator(value, ct);
            result += await validate(value, ct);
            return result;
        };
        return this;
    }

    public string Show() => ShowAsync().GetAwaiter().GetResult();

    public async Task<string> ShowAsync(CancellationToken ct = default) {
        var isValid = false;
        var result = string.Empty;
        while (!isValid) {
            _prompt = $"[teal]{_prompt}[/]";
            output.WriteLine(_prompt);
            output.WriteLine("[gray]Press ENTER to insert a new line and CTRL+ENTER to submit.[/]");
            var editor = new LineEditor {
                MultiLine = true,
                Prompt = new LineEditorPrompt("[yellow]>[/]", "[yellow]|[/]"),
            };
            editor.KeyBindings.Remove(ConsoleKey.Enter);
            editor.KeyBindings.Remove(ConsoleKey.Enter, ConsoleModifiers.Control);
            editor.KeyBindings.Add<NewLineCommand>(ConsoleKey.Enter);
            editor.KeyBindings.Add<SubmitCommand>(ConsoleKey.Enter, ConsoleModifiers.Control);

            result = await editor.ReadLine(ct) ?? string.Empty;
            if (_validator is null) return result;
            var validationResult = await _validator(result, ct);
            if (validationResult.IsSuccess) return result;
            output.WriteLine(validationResult.ToString());
        }
        return result;
    }
}
