namespace DotNetToolbox.Environment;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class AnsiInput()
    : HasDefault<AnsiInput>, IInput {
    private readonly IOutput _output = new AnsiOutput();

    public AnsiInput(IOutput output) : this() {
        _output = output;
    }

    public virtual Encoding Encoding {
        get => Console.InputEncoding;
        set => Console.InputEncoding = value;
    }

    public virtual TextReader Reader => Console.In;

    public virtual bool KeyAvailable() => Console.KeyAvailable;
    public virtual int Read() => Reader.Read();
    public virtual ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);
    public virtual string? ReadLine() => Reader.ReadLine(); // ReadLine is only null when the stream is closed.

    public virtual string ReadText(ConsoleKey submitKey = ConsoleKey.Enter, ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None) {
        var promptLength = Console.CursorLeft;
        var currentLine = new StringBuilder();
        var lines = new List<string>();
        for (var key = ReadKey(intercept: true); !IsSubmitKey(key, submitKey, submitKeyModifiers); key = ReadKey(intercept: true)) {
            if (TryProcessLineBreak(key, currentLine, lines)) continue;
            if (TryProcessBackspace(key, currentLine, lines, promptLength: promptLength)) continue;
            if (TryProcessSpecialKeys(key)) continue;
            TryAddCharacter(key, currentLine);
        }

        AddLineBreak(currentLine);
        lines.Add(currentLine.ToString());
        return string.Join(_output.NewLine, lines);
    }

    private static bool TryProcessSpecialKeys(ConsoleKeyInfo keyInfo)
        => keyInfo.Key is ConsoleKey.LeftArrow or ConsoleKey.RightArrow or ConsoleKey.Home or ConsoleKey.End or ConsoleKey.PageUp or ConsoleKey.PageDown;

    private bool TryProcessBackspace(ConsoleKeyInfo keyInfo, StringBuilder currentLine, List<string> lines, int promptLength) {
        if (keyInfo.Key != ConsoleKey.Backspace) return false;
        if (currentLine.Length > 0) {
            currentLine.Remove(currentLine.Length - 1, 1);
            _output.Write("\b \b");
            return true;
        }

        if (lines.Count == 0) return true;

        currentLine.Append(lines[^1][..^_output.NewLine.Length]);
        lines.RemoveAt(lines.Count - 1);
        Console.CursorTop--;
        Console.CursorLeft = currentLine.Length + (lines.Count == 0 ? promptLength : 0);
        return true;
    }

    private bool TryProcessLineBreak(ConsoleKeyInfo keyInfo, StringBuilder currentLine, List<string> lines) {
        if (!IsLineBreakKey(keyInfo)) return false;
        AddLineBreak(currentLine);
        lines.Add(currentLine.ToString());
        currentLine.Clear();
        return true;
    }

    private static bool IsLineBreakKey(ConsoleKeyInfo keyInfo)
        => keyInfo.Key == ConsoleKey.Enter;

    private static bool IsSubmitKey(ConsoleKeyInfo keyInfo, ConsoleKey submitKey, ConsoleModifiers submitKeyModifiers)
        => keyInfo.Key == submitKey && keyInfo.Modifiers.HasFlag(submitKeyModifiers);

    private bool TryAddCharacter(ConsoleKeyInfo keyInfo, StringBuilder inputBuilder) {
        if (char.IsControl(keyInfo.KeyChar)) return false;
        _output.Write(keyInfo.KeyChar);
        inputBuilder.Append(keyInfo.KeyChar);
        return true;
    }

    private void AddLineBreak(StringBuilder inputBuilder) {
        _output.WriteLine();
        inputBuilder.AppendLine();
    }

    public virtual TextPromptBuilder<TValue> TextPrompt<TValue>(string prompt)
        => new(prompt, _output);
    public virtual TextPromptBuilder<string> TextPrompt(string prompt)
        => new(prompt, _output);

    public virtual TValue Ask<TValue>(string prompt, TValue defaultChoice) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        builder.WithDefault(defaultChoice);
        return builder.Show();
    }
    public virtual TValue Ask<TValue>(string prompt) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        return builder.Show();
    }
    public virtual string Ask(string prompt) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        return builder.Show();
    }

    public virtual bool Confirm(string prompt, bool defaultChoice = true) {
        var builder = new TextPromptBuilder<bool>(prompt, _output);
        builder.AddChoices(true, false);
        builder.ConvertWith(value => value ? "Yes" : "No");
        builder.WithDefault(defaultChoice);
        return builder.Show();
    }

    public virtual SelectionPromptBuilder<TValue> SelectionPrompt<TValue>(string prompt)
        where TValue : notnull
        => new(prompt, _output);

    public virtual SelectionPromptBuilder<string> SelectionPrompt(string prompt)
        => new(prompt, _output);

    public virtual TValue Select<TValue>(string prompt, TValue defaultChoice)
        where TValue : notnull {
        var builder = new SelectionPromptBuilder<TValue>(prompt, _output);
        builder.WithDefault(defaultChoice);
        return builder.Show();
    }
    public virtual TValue Select<TValue>(string prompt)
        where TValue : notnull {
        var builder = new SelectionPromptBuilder<TValue>(prompt, _output);
        return builder.Show();
    }
    public virtual string Select(string prompt) {
        var builder = new SelectionPromptBuilder<string>(prompt, _output);
        return builder.Show();
    }
}
