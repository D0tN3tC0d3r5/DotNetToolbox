namespace DotNetToolbox.Environment;

public interface IInput {
    Encoding Encoding { get; set; }
    TextReader Reader { get; }

    bool KeyAvailable();
    int Read();
    ConsoleKeyInfo ReadKey(bool intercept = false);
    string? ReadLine();
    string ReadMultiLine(ConsoleKey submitKey = ConsoleKey.Enter, ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None);
}
