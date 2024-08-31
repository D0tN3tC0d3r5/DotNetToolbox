using System.Diagnostics.CodeAnalysis;

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
    public void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
        => Lines[^1] += string.Format(format, arg0);
    public void Write([StringSyntax("CompositeFormat")] string format, params object?[] args)
        => Lines[^1] += string.Format(format, args);
    public void Write(string value) {
        var lines = (value ?? string.Empty).Split(System.Environment.NewLine);
        if (lines.Length == 0) return;
        if (Lines.Count == 0) Lines.Add(lines[0]);
        else Lines[^1] += lines[0];
        Lines.AddRange(lines.Skip(1));
    }
    public void WritePrompt() {
        if (Lines.Count == 0) Lines.Add(Prompt);
        else Lines[^1] += Prompt;
    }

    public string NewLine => System.Environment.NewLine;
    public void WriteLine(object? value) {
        Write("{0}", value);
        WriteLine();
    }
    public void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0) {
        Write(format, arg0);
        WriteLine();
    }
    public void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] args) {
        Write(format, args);
        WriteLine();
    }
    public void WriteLine(string value) {
        Write(value ?? string.Empty);
        WriteLine();
    }
    public void Write(bool value) => throw new NotImplementedException();
    public void Write(char value) => throw new NotImplementedException();
    public void Write(char[] buffer, int index, int count) => throw new NotImplementedException();
    public void Write(char[]? buffer) => throw new NotImplementedException();
    public void Write(decimal value) => throw new NotImplementedException();
    public void Write(double value) => throw new NotImplementedException();
    public void Write(float value) => throw new NotImplementedException();
    public void Write(int value) => throw new NotImplementedException();
    public void Write(long value) => throw new NotImplementedException();
    public void Write(object? value) => throw new NotImplementedException();
    public void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1) => throw new NotImplementedException();
    public void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2) => throw new NotImplementedException();
    public void Write(StringBuilder? builder) => throw new NotImplementedException();
    public void Write(uint value) => throw new NotImplementedException();
    public void Write(ulong value) => throw new NotImplementedException();
    public void WriteLine() => throw new NotImplementedException();
    public void WriteLine(bool value) => throw new NotImplementedException();
    public void WriteLine(char value) => throw new NotImplementedException();
    public void WriteLine(char[] buffer, int index, int count) => throw new NotImplementedException();
    public void WriteLine(char[]? buffer) => throw new NotImplementedException();
    public void WriteLine(decimal value) => throw new NotImplementedException();
    public void WriteLine(double value) => throw new NotImplementedException();
    public void WriteLine(float value) => throw new NotImplementedException();
    public void WriteLine(int value) => throw new NotImplementedException();
    public void WriteLine(uint value) => throw new NotImplementedException();
    public void WriteLine(long value) => throw new NotImplementedException();
    public void WriteLine(ulong value) => throw new NotImplementedException();
    public void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1) => throw new NotImplementedException();
    public void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2) => throw new NotImplementedException();
    public void WriteLine(StringBuilder? builder) => throw new NotImplementedException();
    public void WriteOntoNewLine(bool value) => throw new NotImplementedException();
    public void WriteOntoNewLine(char value) => throw new NotImplementedException();
    public void WriteOntoNewLine(char[] buffer, int index, int count) => throw new NotImplementedException();
    public void WriteOntoNewLine(char[]? buffer) => throw new NotImplementedException();
    public void WriteOntoNewLine(decimal value) => throw new NotImplementedException();
    public void WriteOntoNewLine(double value) => throw new NotImplementedException();
    public void WriteOntoNewLine(float value) => throw new NotImplementedException();
    public void WriteOntoNewLine(int value) => throw new NotImplementedException();
    public void WriteOntoNewLine(uint value) => throw new NotImplementedException();
    public void WriteOntoNewLine(long value) => throw new NotImplementedException();
    public void WriteOntoNewLine(ulong value) => throw new NotImplementedException();
    public void WriteOntoNewLine(string value) => throw new NotImplementedException();
    public void WriteOntoNewLine(object? value) => throw new NotImplementedException();
    public void WriteOntoNewLine([StringSyntax("CompositeFormat")] string format, object? arg0) => throw new NotImplementedException();
    public void WriteOntoNewLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1) => throw new NotImplementedException();
    public void WriteOntoNewLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2) => throw new NotImplementedException();
    public void WriteOntoNewLine([StringSyntax("CompositeFormat")] string format, params object?[] args) => throw new NotImplementedException();
    public void WriteOntoNewLine(StringBuilder? builder) => throw new NotImplementedException();
    public void WriteError(Exception exception, string? message = null) => throw new NotImplementedException();
    public void WriteError(string message) => throw new NotImplementedException();
}
