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

    public string ReadText() => string.Empty;
    public Task<string> ReadTextAsync(CancellationToken ct = default) => Task.FromResult(string.Empty);
    public string Prompt(string prompt) => string.Empty;
    public Task<string> PromptAsync(string prompt, CancellationToken ct = default) => Task.FromResult(string.Empty);

    public MultilinePromptBuilder BuildMultilinePrompt(string prompt) => throw new NotImplementedException();

    public TextPromptBuilder<TValue> BuildTextPrompt<TValue>(string prompt) => throw new NotImplementedException();
    public TValue Ask<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices) => throw new NotImplementedException();
    public Task<bool> ConfirmAsync(string prompt, bool defaultChoice, CancellationToken ct = default) => throw new NotImplementedException();

    public TValue Ask<TValue>(string prompt, params TValue[] choices) => throw new NotImplementedException();
    public TValue AskRequired<TValue>(string prompt, params TValue[] choices) => throw new NotImplementedException();
    public string Ask(string prompt, string defaultChoice, params string[] otherChoices) => throw new NotImplementedException();
    public string Ask(string prompt, params string[] choices) => throw new NotImplementedException();
    public string AskRequired(string prompt, params string[] choices) => throw new NotImplementedException();
    public Task<TValue> AskAsync<TValue>(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TValue> AskAsync<TValue>(string prompt, TValue defaultChoice, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TValue> AskAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TValue> AskAsync<TValue>(string prompt, TValue defaultChoice, TValue[] otherChoices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TValue> AskRequiredAsync<TValue>(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TValue> AskRequiredAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> AskAsync(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> AskAsync(string prompt, string defaultChoice, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> AskAsync(string prompt, string[] choices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> AskAsync(string prompt, string defaultChoice, string[] otherChoices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> AskRequiredAsync(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> AskRequiredAsync(string prompt, string[] choices, CancellationToken ct = default) => throw new NotImplementedException();

    public bool Confirm(string prompt, bool defaultChoice = true) => throw new NotImplementedException();
    public Task<bool> ConfirmAsync(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

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

    public Task<TValue> SelectAsync<TValue>(string prompt, TValue defaultChoice, CancellationToken ct = default)
        where TValue : notnull
        => throw new NotImplementedException();

    public Task<TValue> SelectAsync<TValue>(string prompt, CancellationToken ct = default)
        where TValue : notnull
        => throw new NotImplementedException();

    public Task<TValue> SelectAsync<TValue>(string prompt, TValue defaultChoice, TValue[] otherChoices, CancellationToken ct = default)
        where TValue : notnull
        => throw new NotImplementedException();

    public Task<TValue> SelectAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default)
        where TValue : notnull
        => throw new NotImplementedException();

    public Task<TValue> SelectRequiredAsync<TValue>(string prompt, CancellationToken ct = default)
        where TValue : notnull
        => throw new NotImplementedException();

    public Task<TValue> SelectRequiredAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default)
        where TValue : notnull
        => throw new NotImplementedException();

    public Task<string> SelectAsync(string prompt, string defaultChoice, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> SelectAsync(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> SelectAsync(string prompt, string defaultChoice, string[] otherChoices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> SelectAsync(string prompt, string[] choices, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> SelectRequiredAsync(string prompt, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<string> SelectRequiredAsync(string prompt, string[] choices, CancellationToken ct = default) => throw new NotImplementedException();

    public Encoding Encoding {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public TextReader Reader => throw new NotImplementedException();
}
