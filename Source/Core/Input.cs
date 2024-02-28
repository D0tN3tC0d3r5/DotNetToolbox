namespace DotNetToolbox;

public interface IInput {
    Encoding Encoding { get; set; }
    TextReader Reader { get; }

    bool KeyAvailable();
    int Read();
    ConsoleKeyInfo ReadKey(bool intercept = false);
    string? ReadLine();
    string ReadMultiLine(ConsoleKey submitKey = ConsoleKey.Enter, ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None);
}

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class Input() : HasDefault<Input>, IInput {
    private readonly IOutput _output = new Output();

    public Input(IOutput output) : this() {
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
    public virtual string? ReadLine() => Reader.ReadLine()!; // ReadLine is only null when the stream is closed.

    public virtual string ReadMultiLine(ConsoleKey submitKey = ConsoleKey.Enter, ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None) {
        var result = new StringBuilder();
        var keyInfo = ReadKey(intercept: true);
        while (!IsSubmitKey(keyInfo, submitKey, submitKeyModifiers)) {
            if (IsLineBreakKey(keyInfo)) AddLineBreak(result);
            else AddCharacter(result, keyInfo);
            keyInfo = ReadKey(intercept: true);
        }

        return result.ToString();
    }

    private static bool IsLineBreakKey(ConsoleKeyInfo keyInfo)
        => keyInfo.Key == ConsoleKey.Enter;

    private static bool IsSubmitKey(ConsoleKeyInfo keyInfo, ConsoleKey submitKey, ConsoleModifiers submitKeyModifiers)
        => keyInfo.Key == submitKey && keyInfo.Modifiers.HasFlag(submitKeyModifiers);

    private void AddCharacter(StringBuilder inputBuilder, ConsoleKeyInfo keyInfo) {
        _output.Write(keyInfo.KeyChar);
        inputBuilder.Append(keyInfo.KeyChar);
    }

    private void AddLineBreak(StringBuilder inputBuilder) {
        _output.WriteLine();
        inputBuilder.AppendLine();
    }
}
