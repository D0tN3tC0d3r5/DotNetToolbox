using Spectre.Console.Rendering;

namespace DotNetToolbox.Environment;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class AnsiOutput
    : HasDefault<AnsiOutput>,
      IOutput {
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

    public virtual void Write(bool value)
        => AnsiConsole.Write(value);
    public virtual void Write(ulong value)
        => AnsiConsole.Write(value);
    public virtual void Write(uint value)
        => AnsiConsole.Write(value);
    public virtual void Write(long value)
        => AnsiConsole.Write(value);
    public virtual void Write(int value)
        => AnsiConsole.Write(value);
    public virtual void Write(float value)
        => AnsiConsole.Write(value);
    public virtual void Write(double value)
        => AnsiConsole.Write(value);
    public virtual void Write(decimal value)
        => AnsiConsole.Write(value);
    public virtual void Write(char value)
        => AnsiConsole.Write(value);
    public virtual void Write(string value)
        => AnsiConsole.Markup(value);
    public virtual void Write(object? value) {
        if (value is IRenderable renderable) AnsiConsole.Write(renderable);
        else AnsiConsole.Markup($"{value}");
    }
    public virtual void Write(StringBuilder? builder)
        => AnsiConsole.Markup(builder?.ToString() ?? string.Empty);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0)
        => AnsiConsole.Markup(format, [arg0!]);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1)
        => AnsiConsole.Markup(format, [arg0!, arg1!]);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
        => AnsiConsole.Markup(format, [arg0!, arg1!, arg2!]);
    public virtual void Write([Syntax(Syntax.CompositeFormat)] string format, params object?[] args)
        => AnsiConsole.Markup(format, args!);
    public virtual void Write(char[]? buffer)
        => AnsiConsole.Markup(new string(buffer));
    public virtual void Write(char[] buffer, int index, int count)
        => AnsiConsole.Markup(new string(buffer), index, count);

    public virtual void WritePrompt()
        => AnsiConsole.Markup(Prompt);
    public virtual string NewLine => System.Environment.NewLine;

    public virtual void WriteLine()
        => AnsiConsole.WriteLine();
    public virtual void WriteLine(bool value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(ulong value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(uint value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(long value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(int value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(float value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(double value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(decimal value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(char value)
        => AnsiConsole.WriteLine(value);
    public virtual void WriteLine(string value)
        => AnsiConsole.MarkupLine(value);
    public virtual void WriteLine(object? value) {
        if (value is IRenderable renderable) AnsiConsole.Write(renderable);
        else AnsiConsole.MarkupLine($"{value}");
    }
    public virtual void WriteLine(StringBuilder? builder)
        => AnsiConsole.MarkupLine(builder?.ToString() ?? string.Empty);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0)
        => AnsiConsole.MarkupLine(format, arg0!);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1)
        => AnsiConsole.MarkupLine(format, arg0!, arg1!);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
        => AnsiConsole.MarkupLine(format, arg0!, arg1!, arg2!);
    public virtual void WriteLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args)
        => AnsiConsole.MarkupLine(format, args!);
    public virtual void WriteLine(char[]? buffer)
        => AnsiConsole.MarkupLine(new string(buffer));
    public virtual void WriteLine(char[] buffer, int index, int count)
        => AnsiConsole.MarkupLine(new string(buffer), index, count);

    public virtual void WriteOntoNewLine(bool value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(uint value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(ulong value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(int value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(long value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(float value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(double value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(decimal value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(char value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(string value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(object? value) {
        WriteLine();
        Write(value);
    }
    public virtual void WriteOntoNewLine(StringBuilder? builder) {
        WriteLine();
        Write(builder);
    }
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0) {
        WriteLine();
        Write(format, arg0);
    }
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1) {
        WriteLine();
        Write(format, arg0, arg1);
    }
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) {
        WriteLine();
        Write(format, arg0, arg1, arg2);
    }
    public virtual void WriteOntoNewLine([Syntax(Syntax.CompositeFormat)] string format, params object?[] args) {
        WriteLine();
        Write(format, args);
    }
    public virtual void WriteOntoNewLine(char[]? buffer) {
        WriteLine();
        Write(buffer);
    }
    public virtual void WriteOntoNewLine(char[] buffer, int index, int count) {
        WriteLine();
        Write(buffer, index, count);
    }

#if DEBUG
    private const ExceptionFormats _exceptionFormat = ExceptionFormats.ShowLinks;
# else
    private const ExceptionFormats _exceptionFormat = ExceptionFormats.ShowLinks
                                                    | ExceptionFormats.ShortenEverything
                                                    | ExceptionFormats.NoStackTrace;
#endif

    public virtual void WriteError(Exception exception, string? message = null) {
        WriteLine();
        WriteLine($"[bold red]{message ?? "An error has occurred."}[/]");
        AnsiConsole.WriteException(exception, _exceptionFormat);
        WriteLine();
    }

    public virtual void WriteError(string message) {
        WriteLine();
        WriteLine($"[bold red]An error has occurred:[/] {IsNotNullOrWhiteSpace(message)}");
        WriteLine();
    }
}
