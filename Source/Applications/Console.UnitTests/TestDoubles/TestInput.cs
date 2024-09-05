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

    public TextPromptBuilder<TValue> BuildTextPrompt<TValue>(string prompt) => throw new NotImplementedException();
    public TValue Ask<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices) => throw new NotImplementedException();
    public TValue Ask<TValue>(string prompt, params TValue[] choices) => throw new NotImplementedException();
    public TValue AskRequired<TValue>(string prompt, params TValue[] choices) => throw new NotImplementedException();
    public string Ask(string prompt, string defaultChoice, params string[] otherChoices) => throw new NotImplementedException();
    public string Ask(string prompt, params string[] choices) => throw new NotImplementedException();
    public string AskRequired(string prompt, params string[] choices) => throw new NotImplementedException();
    public bool Confirm(string prompt, bool defaultChoice = true) => throw new NotImplementedException();
    public SelectionPromptBuilder<TValue> BuildSelectionPrompt<TValue>(string prompt)
        where TValue : notnull
        => throw new NotImplementedException();
    public TValue Select<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices)
        where TValue : notnull
        => throw new NotImplementedException();
    public TValue Select<TValue>(string prompt, params TValue[] choices)
        where TValue : notnull
        => throw new NotImplementedException();
    public TValue SelectRequired<TValue>(string prompt, params TValue[] choices)
        where TValue : notnull
        => throw new NotImplementedException();
    public string Select(string prompt, string defaultChoice, params string[] otherChoices)
        => throw new NotImplementedException();
    public string Select(string prompt, params string[] choices)
        => throw new NotImplementedException();
    public string SelectRequired(string prompt, params string[] choices)
        => throw new NotImplementedException();

    public Encoding Encoding {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public TextReader Reader => throw new NotImplementedException();
}
