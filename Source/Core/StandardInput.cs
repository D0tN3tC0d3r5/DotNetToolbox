namespace DotNetToolbox;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class StandardInput {
    public virtual int Read() => Console.Read();
    public virtual ConsoleKeyInfo ReadKey() => Console.ReadKey();
    public virtual string? ReadLine() => Console.ReadLine();
}
