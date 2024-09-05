namespace DotNetToolbox.Environment;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class ConsoleInput()
    : HasDefault<ConsoleInput>, IInput {
    private readonly IOutput _output = new ConsoleOutput();

    public ConsoleInput(IOutput output) : this() {
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

    public virtual TValue Ask<TValue>(string prompt, params TValue[] choices) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.Show();
    }
    public virtual TValue Ask<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        if (otherChoices.Length > 0) builder.AddChoices([defaultChoice, ..otherChoices]);
        builder.WithDefault(defaultChoice);
        return builder.Show();
    }
    public virtual string Ask(string prompt, params string[] choices) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.Show();
    }
    public virtual string Ask(string prompt, string defaultChoice, params string[] otherChoices) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        if (otherChoices.Length > 0) builder.AddChoices([defaultChoice, ..otherChoices]);
        builder.WithDefault(defaultChoice);
        return builder.Show();
    }
    public virtual TValue AskRequired<TValue>(string prompt, params TValue[] choices) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        builder.AsRequired();
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.Show();
    }
    public virtual string AskRequired(string prompt, params string[] choices) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        builder.AsRequired();
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.Show();
    }

    public virtual bool Confirm(string prompt, bool defaultChoice = true) {
        var builder = new TextPromptBuilder<bool>(prompt, _output);
        builder.AddChoices(true, false);
        builder.ConvertWith(value => value ? "Yes" : "No");
        builder.WithDefault(defaultChoice);
        return builder.Show();
    }

    public virtual TextPromptBuilder<TValue> BuildTextPrompt<TValue>(string prompt)
        => new(prompt, _output);

    public virtual SelectionPromptBuilder<TValue> BuildSelectionPrompt<TValue>(string prompt)
        where TValue : notnull
        => new(prompt, _output);

    public virtual TValue Select<TValue>(string prompt, params TValue[] choices)
        where TValue : notnull {
        var builder = new SelectionPromptBuilder<TValue>(prompt, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return builder.Show();
    }
    public virtual TValue Select<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices)
        where TValue : notnull {
        var builder = new SelectionPromptBuilder<TValue>(prompt, _output);
        builder.WithDefault(defaultChoice);
        if (otherChoices.Length < 1) throw new ArgumentException("At least two choices must be provided.", nameof(otherChoices));
        builder.AddChoices([defaultChoice, ..otherChoices]);
        return builder.Show();
    }
    public virtual string Select(string prompt, params string[] choices) {
        var builder = new SelectionPromptBuilder<string>(prompt, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return builder.Show();
    }
    public virtual string Select(string prompt, string defaultChoice, params string[] otherChoices) {
        var builder = new SelectionPromptBuilder<string>(prompt, _output);
        builder.WithDefault(defaultChoice);
        if (otherChoices.Length < 1) throw new ArgumentException("At least two choices must be provided.", nameof(otherChoices));
        builder.AddChoices([defaultChoice, ..otherChoices]);
        return builder.Show();
    }
    public virtual TValue SelectRequired<TValue>(string prompt, params TValue[] choices)
        where TValue : notnull {
        var builder = new SelectionPromptBuilder<TValue>(prompt, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return builder.Show();
    }
    public virtual string SelectRequired(string prompt, params string[] choices) {
        var builder = new SelectionPromptBuilder<string>(prompt, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return builder.Show();
    }
}
