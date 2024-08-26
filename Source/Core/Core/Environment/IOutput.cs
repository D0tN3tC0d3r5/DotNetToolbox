namespace DotNetToolbox.Environment;

public interface IOutput {
    string Prompt { get; set; }
    ConsoleColor BackgroundColor { get; set; }
    Encoding Encoding { get; set; }
    TextWriter ErrorWriter { get; }
    ConsoleColor ForegroundColor { get; set; }
    TextWriter Writer { get; }

    void ClearScreen();
    void ResetColor();
    void Write(bool value);
    void Write(char value);
    void Write(char[] buffer, int index, int count);
    void Write(char[]? buffer);
    void Write(decimal value);
    void Write(double value);
    void Write(float value);
    void Write(int value);
    void Write(long value);
    void Write(object? value);
    void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0);
    void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1);
    void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2);
    void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] args);
    void Write(string? value);
    void Write(StringBuilder? builder);
    void Write(uint value);
    void Write(ulong value);
    void WritePrompt();

    string NewLine { get; }
    void WriteLine();
    void WriteLine(bool value);
    void WriteLine(char value);
    void WriteLine(char[] buffer, int index, int count);
    void WriteLine(char[]? buffer);
    void WriteLine(decimal value);
    void WriteLine(double value);
    void WriteLine(float value);
    void WriteLine(int value);
    void WriteLine(uint value);
    void WriteLine(long value);
    void WriteLine(ulong value);
    void WriteLine(object? value);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg);
    void WriteLine(string? value);
    void WriteLine(StringBuilder? builder);
    void WriteOntoNewLine(bool value);
    void WriteOntoNewLine(char value);
    void WriteOntoNewLine(char[] buffer, int index, int count);
    void WriteOntoNewLine(char[]? buffer);
    void WriteOntoNewLine(decimal value);
    void WriteOntoNewLine(double value);
    void WriteOntoNewLine(float value);
    void WriteOntoNewLine(int value);
    void WriteOntoNewLine(uint value);
    void WriteOntoNewLine(long value);
    void WriteOntoNewLine(ulong value);
    void WriteOntoNewLine(object? value);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args);
    void WriteOntoNewLine(string? value);
    void WriteOntoNewLine(StringBuilder? builder);
    void WriteError(Exception exception);
}
