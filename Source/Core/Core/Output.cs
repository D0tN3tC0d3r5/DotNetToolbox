namespace DotNetToolbox;

public interface IOutput {
    string Prompt { get; set; }
    ConsoleColor BackgroundColor { get; set; }
    Encoding Encoding { get; set; }
    TextWriter Error { get; }
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
    void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg);
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
    void WriteLine(long value);
    void WriteLine(object? value);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2);
    void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg);
    void WriteLine(string? value);
    void WriteLine(StringBuilder? builder);
    void WriteLine(uint value);
    void WriteLine(ulong value);
}

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class Output : HasDefault<Output>, IOutput {
    public virtual string Prompt { get; set; } = "> ";

    public virtual Encoding Encoding {
        get => Console.OutputEncoding;
        set => Console.OutputEncoding = value;
    }

    public virtual ConsoleColor ForegroundColor {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }
    public virtual ConsoleColor BackgroundColor {
        get => Console.BackgroundColor;
        set => Console.BackgroundColor = value;
    }

    public virtual TextWriter Writer => Console.Out;
    public virtual TextWriter Error => Console.Error;

    public virtual void ResetColor() => Console.ResetColor();
    public virtual void ClearScreen() => Console.Clear();

    public virtual void Write(bool value) => Console.Write(value);
    public virtual void Write(ulong value) => Console.Write(value);
    public virtual void Write(uint value) => Console.Write(value);
    public virtual void Write(long value) => Console.Write(value);
    public virtual void Write(int value) => Console.Write(value);
    public virtual void Write(float value) => Console.Write(value);
    public virtual void Write(double value) => Console.Write(value);
    public virtual void Write(decimal value) => Console.Write(value);
    public virtual void Write(char value) => Console.Write(value);
    public virtual void Write(string? value) => Console.Write(value);
    public virtual void Write(object? value) => Console.Write(value);

    public virtual void Write(StringBuilder? builder) => Console.Write(builder);

    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0) => Console.Write(format, arg0);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1) => Console.Write(format, arg0, arg1);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => Console.Write(format, arg0, arg1, arg2);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg) => Console.Write(format, arg);

    public virtual void Write(char[]? buffer) => Console.Write(buffer);
    public virtual void Write(char[] buffer, int index, int count) => Console.Write(buffer, index, count);

    public virtual void WritePrompt() => Console.Write(Prompt);
    public virtual string NewLine => System.Environment.NewLine;
    public virtual void WriteLine() => Console.WriteLine();

    public virtual void WriteLine(bool value) => Console.WriteLine(value);
    public virtual void WriteLine(uint value) => Console.WriteLine(value);
    public virtual void WriteLine(ulong value) => Console.WriteLine(value);
    public virtual void WriteLine(int value) => Console.WriteLine(value);
    public virtual void WriteLine(long value) => Console.WriteLine(value);
    public virtual void WriteLine(float value) => Console.WriteLine(value);
    public virtual void WriteLine(double value) => Console.WriteLine(value);
    public virtual void WriteLine(decimal value) => Console.WriteLine(value);
    public virtual void WriteLine(char value) => Console.WriteLine(value);
    public virtual void WriteLine(string? value) => Console.WriteLine(value);
    public virtual void WriteLine(object? value) => Console.WriteLine(value);

    public virtual void WriteLine(StringBuilder? builder) => Console.WriteLine(builder);

    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0) => Console.WriteLine(format, arg0);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1) => Console.WriteLine(format, arg0, arg1);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => Console.WriteLine(format, arg0, arg1, arg2);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg) => Console.WriteLine(format, arg);

    public virtual void WriteLine(char[]? buffer) => Console.WriteLine(buffer);
    public virtual void WriteLine(char[] buffer, int index, int count) => Console.WriteLine(buffer, index, count);
}
