namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal sealed class TestOutput : IOutput {
    public string Prompt { get; set; } = "> ";
    public override string ToString() => string.Join(NewLine, Lines);
    public List<string> Lines { get; } = [];

    public TextWriter ErrorWriter => throw new NotImplementedException();
    public TextWriter Writer => throw new NotImplementedException();

    public Encoding Encoding {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public ConsoleColor ForegroundColor {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public ConsoleColor BackgroundColor {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public void ClearScreen() {
        Lines.Clear();
        Lines.Add(string.Empty);
    }

    public void ResetColor() => throw new NotImplementedException();

    public void Write(bool value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(char value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(decimal value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(double value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(float value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(int value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(long value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(object? value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default)
        => Lines[^1] += string.Format(format, arg0);
    public void Write(string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] args)
        => Lines[^1] += string.Format(format, args);
    public void Write(string? value, ConsoleColor foreground = default, ConsoleColor background = default) {
        var lines = (value ?? string.Empty).Split(System.Environment.NewLine);
        if (lines.Length == 0) return;
        if (Lines.Count == 0) Lines.Add(lines[0]);
        else Lines[^1] += lines[0];
        Lines.AddRange(lines.Skip(1));
    }
    public void Write(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(uint value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void Write(ulong value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();

    public void WritePrompt(ConsoleColor foreground = default, ConsoleColor background = default) {
        if (Lines.Count == 0) Lines.Add(Prompt);
        else Lines[^1] += Prompt;
    }

    public string NewLine => System.Environment.NewLine;

    public void WriteLine(ConsoleColor foreground = default, ConsoleColor background = default) => Lines.Add(string.Empty);
    public void WriteLine(bool value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(char value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(decimal value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(double value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(float value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(int value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(uint value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(long value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(ulong value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(object? value, ConsoleColor foreground = default, ConsoleColor background = default) {
        Write("{0}", value);
        WriteLine();
    }
    public void WriteLine(string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default) {
        Write(format, arg0);
        WriteLine();
    }
    public void WriteLine(string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine(string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) {
        Write(format, args);
        WriteLine();
    }
    public void WriteLine(string? value, ConsoleColor foreground = default, ConsoleColor background = default) {
        Write(value ?? string.Empty);
        WriteLine();
    }
    public void WriteLine(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();

    public void WriteOntoNewLine(bool value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(char value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(decimal value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(double value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(float value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(int value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(uint value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(long value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(ulong value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(object? value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(string format, params object?[] arg) => throw new NotImplementedException();
    public void WriteOntoNewLine(string? value, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
    public void WriteOntoNewLine(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default) => throw new NotImplementedException();
}
