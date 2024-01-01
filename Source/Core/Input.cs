namespace DotNetToolbox;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class Input {
    public virtual Encoding Encoding {
        get => Console.InputEncoding;
        set => Console.InputEncoding = value;
    }

    public virtual TextReader Reader => Console.In;

    public virtual bool KeyAvailable() => Console.KeyAvailable;
    public virtual int Read() => Console.Read();
    public virtual ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);
    public virtual string? ReadLine() => Console.ReadLine();
}
