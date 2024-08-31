namespace DotNetToolbox.Environment;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class ConsoleOutput
    : HasDefault<ConsoleOutput>,
      IOutput {
    public const ConsoleColor DefaultForeground = ConsoleColor.White;
    public const ConsoleColor DefaultBackground = ConsoleColor.Black;
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

    void IOutput.Write(bool value) => Write(value);
    public virtual void Write(bool value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(ulong value) => Write(value);
    public virtual void Write(ulong value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(uint value) => Write(value);
    public virtual void Write(uint value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(long value) => Write(value);
    public virtual void Write(long value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(int value) => Write(value);
    public virtual void Write(int value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(float value) => Write(value);
    public virtual void Write(float value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(double value) => Write(value);
    public virtual void Write(double value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(decimal value) => Write(value);
    public virtual void Write(decimal value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(char value) => Write(value);
    public virtual void Write(char value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(string value) => Write(value);
    public virtual void Write(string value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(object? value) => Write(value);
    public virtual void Write(object? value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(value), foreground, background);
    void IOutput.Write(StringBuilder? builder) => Write(builder);
    public virtual void Write(StringBuilder? builder, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(builder), foreground, background);
    void IOutput.Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0) => Write(format, arg0);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(format, arg0), foreground, background);
    void IOutput.Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1) => Write(format, arg0, arg1);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(format, arg0, arg1), foreground, background);
    void IOutput.Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => Write(format, arg0, arg1, arg2);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(format, arg0, arg1, arg2), foreground, background);
    void IOutput.Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) => Write(format, args);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) => Console.Write(format, args);
    void IOutput.Write(char[]? buffer) => Write(buffer);
    public virtual void Write(char[]? buffer, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(buffer), foreground, background);
    void IOutput.Write(char[] buffer, int index, int count) => Write(buffer, index, count);
    public virtual void Write(char[] buffer, int index, int count, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(buffer, index, count), foreground, background);

    void IOutput.WritePrompt() => WritePrompt();
    public virtual void WritePrompt(ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.Write(Prompt), foreground, background);
    public virtual string NewLine => System.Environment.NewLine;

    void IOutput.WriteLine() => WriteLine();
    public virtual void WriteLine(ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(Console.WriteLine, foreground, background);
    void IOutput.WriteLine(bool value) => WriteLine(value);
    public virtual void WriteLine(bool value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(ulong value) => WriteLine(value);
    public virtual void WriteLine(ulong value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(uint value) => WriteLine(value);
    public virtual void WriteLine(uint value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(long value) => WriteLine(value);
    public virtual void WriteLine(long value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(int value) => WriteLine(value);
    public virtual void WriteLine(int value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(float value) => WriteLine(value);
    public virtual void WriteLine(float value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(double value) => WriteLine(value);
    public virtual void WriteLine(double value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(decimal value) => WriteLine(value);
    public virtual void WriteLine(decimal value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(char value) => WriteLine(value);
    public virtual void WriteLine(char value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(string value) => WriteLine(value);
    public virtual void WriteLine(string value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(object? value) => WriteLine(value);
    public virtual void WriteLine(object? value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(value), foreground, background);
    void IOutput.WriteLine(StringBuilder? builder) => WriteLine(builder);
    public virtual void WriteLine(StringBuilder? builder, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(builder), foreground, background);
    void IOutput.WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0) => WriteLine(format, arg0);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(format, arg0), foreground, background);
    void IOutput.WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1) => WriteLine(format, arg0, arg1);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(format, arg0, arg1), foreground, background);
    void IOutput.WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => WriteLine(format, arg0, arg1, arg2);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(format, arg0, arg1, arg2), foreground, background);
    void IOutput.WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) => WriteLine(format, args);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) => Console.WriteLine(format, args);
    void IOutput.WriteLine(char[]? buffer) => WriteLine(buffer);
    public virtual void WriteLine(char[]? buffer, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(buffer), foreground, background);
    void IOutput.WriteLine(char[] buffer, int index, int count) => WriteLine(buffer, index, count);
    public virtual void WriteLine(char[] buffer, int index, int count, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground)
        => WithColor(() => Console.WriteLine(buffer, index, count), foreground, background);

    void IOutput.WriteOntoNewLine(bool value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(bool value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(uint value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(uint value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(ulong value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(ulong value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(int value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(int value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(long value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(long value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(float value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(float value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(double value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(double value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(decimal value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(decimal value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(char value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(char value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(string value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(string value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(object? value) => WriteOntoNewLine(value);
    public virtual void WriteOntoNewLine(object? value, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(value, foreground, background);
    }
    void IOutput.WriteOntoNewLine(StringBuilder? builder) => WriteOntoNewLine(builder);
    public virtual void WriteOntoNewLine(StringBuilder? builder, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(builder, foreground, background);
    }
    void IOutput.WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0) => WriteOntoNewLine(format, arg0);
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(format, arg0, foreground, background);
    }
    void IOutput.WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1) => WriteOntoNewLine(format, arg0, arg1);
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(format, arg0, arg1, foreground, background);
    }
    void IOutput.WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => WriteOntoNewLine(format, arg0, arg1, arg2);
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(format, arg0, arg1, arg2, foreground, background);
    }
    void IOutput.WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object?[] args) => WriteOntoNewLine(format, args);
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) {
        WriteLine();
        Write(format, args);
    }
    void IOutput.WriteOntoNewLine(char[]? buffer) => WriteOntoNewLine(buffer);
    public virtual void WriteOntoNewLine(char[]? buffer, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(buffer, foreground, background);
    }
    void IOutput.WriteOntoNewLine(char[] buffer, int index, int count) => WriteOntoNewLine(buffer, index, count);
    public virtual void WriteOntoNewLine(char[] buffer, int index, int count, ConsoleColor foreground = DefaultForeground, ConsoleColor background = DefaultBackground) {
        WriteLine(foreground, background);
        Write(buffer, index, count, foreground, background);
    }

    public virtual void WriteError(Exception exception, string? message = null) {
        WriteLine(message ?? "An error has occurred.");
        WriteLine(exception.ToString());
    }

    public virtual void WriteError(string message) {
        WriteLine();
        WriteLine($"An error has occurred: {IsNotNullOrWhiteSpace(message)}");
        WriteLine();
    }

    private void WithColor(Action method, ConsoleColor foreground, ConsoleColor background) {
        var previous = (ForegroundColor, BackgroundColor);
        (ForegroundColor, BackgroundColor) = (foreground, background);
        method();
        (ForegroundColor, BackgroundColor) = previous;
    }
}
