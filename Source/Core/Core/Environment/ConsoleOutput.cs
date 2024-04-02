namespace DotNetToolbox.Environment;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class ConsoleOutput : HasDefault<ConsoleOutput>, IOutput {
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
    public virtual TextWriter ErrorWriter => Console.Error;

    public virtual void ResetColor() => Console.ResetColor();
    public virtual void ClearScreen() => Console.Clear();

    public virtual void Write(bool value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);

    public virtual void Write(ulong value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(uint value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(long value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(int value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(float value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(double value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(decimal value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(char value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(string? value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);
    public virtual void Write(object? value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(value), foreground, background);

    public virtual void Write(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(builder), foreground, background);

    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(format, arg0), foreground, background);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(format, arg0, arg1), foreground, background);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(format, arg0, arg1, arg2), foreground, background);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg) => Console.Write(format, arg);

    public virtual void Write(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(buffer), foreground, background);
    public virtual void Write(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(buffer, index, count), foreground, background);

    public virtual void WritePrompt(ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.Write(Prompt), foreground, background);
    public virtual string NewLine => System.Environment.NewLine;
    public virtual void WriteLine(ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(Console.WriteLine, foreground, background);

    public virtual void WriteLine(bool value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(uint value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(ulong value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(int value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(long value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(float value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(double value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(decimal value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(char value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(string? value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    public virtual void WriteLine(object? value, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(value), foreground, background);

    public virtual void WriteLine(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(builder), foreground, background);

    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(format, arg0), foreground, background);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(format, arg0, arg1), foreground, background);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(format, arg0, arg1, arg2), foreground, background);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] arg) => Console.WriteLine(format, arg);

    public virtual void WriteLine(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(buffer), foreground, background);
    public virtual void WriteLine(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default)
        => WithColor(() => Console.WriteLine(buffer, index, count), foreground, background);

    public virtual void WriteOntoNewLine(bool value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(uint value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(ulong value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(int value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(long value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(float value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(double value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(decimal value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(char value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(string? value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(object? value, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }

    public virtual void WriteOntoNewLine(StringBuilder? builder, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(builder, foreground, background);
    }

    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(format, arg0, foreground, background);
    }

    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(format, arg0, arg1, foreground, background);
    }

    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(format, arg0, arg1, arg2, foreground, background);
    }

    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) {
        WriteLine();
        Write(format, args);
    }

    public virtual void WriteOntoNewLine(char[]? buffer, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(buffer, foreground, background);
    }

    public virtual void WriteOntoNewLine(char[] buffer, int index, int count, ConsoleColor foreground = default, ConsoleColor background = default) {
        WriteLine(foreground, background);
        Write(buffer, index, count, foreground, background);
    }

    private void WithColor(Action method, ConsoleColor foreground, ConsoleColor background) {
        var previous = (ForegroundColor, BackgroundColor);
        (ForegroundColor, BackgroundColor) = (foreground, background);
        method();
        (ForegroundColor, BackgroundColor) = previous;
    }
}
