namespace DotNetToolbox.Environment;

public class MultilinePromptBuilder(string prompt, IOutput output)
    : IMultilinePromptBuilder {
    private string _prompt = prompt;

    public MultilinePromptBuilder(IOutput output)
        : this(string.Empty, output) {
    }

    public string Show() => ShowAsync().GetAwaiter().GetResult();

    public async Task<string> ShowAsync(CancellationToken ct = default) {
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

        return await editor.ReadLine(ct) ?? string.Empty;
    }
}
