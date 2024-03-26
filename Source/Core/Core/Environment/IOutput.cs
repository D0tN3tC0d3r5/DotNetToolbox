namespace DotNetToolbox.Environment;

public interface IOutput {
    string Prompt { get; set; }
    ConsoleColor BackgroundColor { get; set; }
    Encoding Encoding { get; set; }
    TextWriter Error { get; }
    ConsoleColor ForegroundColor { get; set; }
    TextWriter Writer { get; }

    void ClearScreen();
    void ResetColor();
    void Write(bool value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(char value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(decimal value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(double value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(float value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(int value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(long value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(object? value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg);
    void Write(string? value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(uint value, ConsoleColor foreground = default, ConsoleColor background = default);
    void Write(ulong value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WritePrompt(ConsoleColor foreground = default, ConsoleColor background = default);

    string NewLine { get; }
    void WriteLine(ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(bool value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(char value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(decimal value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(double value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(float value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(int value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(uint value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(long value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(ulong value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(object? value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg);
    void WriteLine(string? value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteLine(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(bool value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(char value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(decimal value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(double value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(float value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(int value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(uint value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(long value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(ulong value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(object? value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg);
    void WriteOntoNewLine(string? value, ConsoleColor foreground = default, ConsoleColor background = default);
    void WriteOntoNewLine(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default);
}
