namespace DotNetToolbox.Environment;

public class MultilinePromptBuilder(string prompt, IOutput output)
    : IMultilinePromptBuilder {
    private string _prompt = prompt;
    private Func<string, Result>? _validator;

    public MultilinePromptBuilder(IOutput output)
        : this(string.Empty, output) {
    }

    public MultilinePromptBuilder Validate(Func<string, Result> validate) {
        var oldValidator = _validator;
        _validator = value => {
            var result = oldValidator?.Invoke(value) ?? Result.Success();
            result += validate(value);
            return result;
        };
        return this;
    }

    public string Show() => ShowAsync().GetAwaiter().GetResult();

    public async Task<string> ShowAsync(CancellationToken ct = default) {
        while (true) {
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

            var result = await editor.ReadLine(ct) ?? string.Empty;
            if (_validator is null) return result;
            var validationResult = _validator(result);
            if (validationResult.IsSuccess) return result;
            foreach (var validationResultError in validationResult.Errors)
                output.WriteLine($"[red]{validationResultError.Message}[/]");
            output.WriteLine("[yellow]Please try again.[/]");
            output.WriteLine();
        }
    }
}
