namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal sealed class TestInput(IOutput output, params string[] inputs) : IInput {
    private readonly Queue<string> _inputQueue = new(inputs);

    public bool KeyAvailable() => throw new NotImplementedException();

    public int Read() => throw new NotImplementedException();

    public ConsoleKeyInfo ReadKey(bool intercept = false) => throw new NotImplementedException();

    public string ReadLine() {
        if (!_inputQueue.TryDequeue(out var input)) return string.Empty;
        output.WriteLine(input);
        return input;
    }

    public string ReadMultiLine(ConsoleKey submitKey = ConsoleKey.Enter,
                                ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None) {
        var input = string.Empty;
        while (_inputQueue.TryDequeue(out var line) && !line.EndsWith("[Ctrl+Enter]")) {
            output.WriteLine(line);
            input += line + System.Environment.NewLine;
        }
        return input;
    }

    public Encoding Encoding {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public TextReader Reader => throw new NotImplementedException();
}
