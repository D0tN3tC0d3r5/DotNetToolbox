namespace DotNetToolbox;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class OutputWriter {
    public bool UseColors { get; set; } = true;
    public OutputVerboseLevel VerboseLevel { get; set; } = OutputVerboseLevel.Normal;

    public virtual ConsoleColor ForegroundColor {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }

    public virtual ConsoleColor BackgroundColor {
        get => Console.BackgroundColor;
        set => Console.BackgroundColor = value;
    }

    public virtual void ResetColor() => Console.ResetColor();

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

    public virtual void Write(string format, object? arg0) => Console.Write(format, arg0);
    public virtual void Write(string format, object? arg0, object? arg1) => Console.Write(format, arg0, arg1);
    public virtual void Write(string format, object? arg0, object? arg1, object? arg2) => Console.Write(format, arg0, arg1, arg2);
    public virtual void Write(string format, params object?[] arg) => Console.Write(format, arg);

    public virtual void Write(char[]? buffer) => Console.Write(buffer);
    public virtual void Write(char[] buffer, int index, int count) => Console.Write(buffer, index, count);

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

    public virtual void WriteLine(string format, object? arg0) => Console.WriteLine(format, arg0);
    public virtual void WriteLine(string format, object? arg0, object? arg1) => Console.WriteLine(format, arg0, arg1);
    public virtual void WriteLine(string format, object? arg0, object? arg1, object? arg2) => Console.WriteLine(format, arg0, arg1, arg2);
    public virtual void WriteLine(string format, params object?[] arg) => Console.WriteLine(format, arg);

    public virtual void WriteLine(char[]? buffer) => Console.WriteLine(buffer);
    public virtual void WriteLine(char[] buffer, int index, int count) => Console.WriteLine(buffer, index, count);
}
