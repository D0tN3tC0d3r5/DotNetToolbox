namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal sealed class TestInput(IOutput output, params string[] inputs)
    : IInput {
    private readonly Queue<string> _inputQueue = new(inputs);

    public bool KeyAvailable() => throw new NotImplementedException();

    public int Read() => throw new NotImplementedException();

    public ConsoleKeyInfo ReadKey(bool intercept = false) => throw new NotImplementedException();

    public string ReadLine() {
        if (!_inputQueue.TryDequeue(out var input)) return string.Empty;
        output.WriteLine(input);
        return input;
    }

    public string ReadText(ConsoleKey submitKey = ConsoleKey.Enter,
                                ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None) {
        var input = string.Empty;
        while (_inputQueue.TryDequeue(out var line) && !line.EndsWith("[Ctrl+Enter]")) {
            output.WriteLine(line);
            input += line + System.Environment.NewLine;
        }
        return input;
    }

    public TextPromptBuilder<TValue> TextPrompt<TValue>(string prompt) => throw new NotImplementedException();
    public TextPromptBuilder<string> TextPrompt(string prompt) => throw new NotImplementedException();
    public TValue Ask<TValue>(string prompt, TValue defaultChoice) => throw new NotImplementedException();
    public TValue Ask<TValue>(string prompt) => throw new NotImplementedException();
    public string Ask(string prompt) => throw new NotImplementedException();
    public bool Confirm(string prompt, bool defaultChoice = true) => throw new NotImplementedException();
    public SelectionPromptBuilder<TValue> SelectionPrompt<TValue>(string prompt)
        where TValue : notnull
        => throw new NotImplementedException();
    public SelectionPromptBuilder<string> SelectionPrompt(string prompt) => throw new NotImplementedException();
    public TValue Select<TValue>(string prompt, TValue defaultChoice)
        where TValue : notnull
        => throw new NotImplementedException();
    public TValue Select<TValue>(string prompt)
        where TValue : notnull
        => throw new NotImplementedException();
    public string Select(string prompt) => throw new NotImplementedException();

    public Encoding Encoding {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public TextReader Reader => throw new NotImplementedException();
}
