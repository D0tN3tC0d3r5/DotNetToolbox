namespace DotNetToolbox.Environment;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Console functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for externally.
public class ConsoleInput()
    : HasDefault<ConsoleInput>,
      IInput {
    private readonly IOutput _output = new ConsoleOutput();

    public ConsoleInput(IOutput output) : this() {
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
    public virtual string? ReadLine() => Reader.ReadLine(); // ReadLine is only null when the stream is closed.

    public virtual MultilinePromptBuilder BuildMultilinePrompt(string prompt)
        => new(prompt, _output);
    public virtual TextPromptBuilder<TValue> BuildTextPrompt<TValue>(string prompt)
        => new(prompt, _output);

    public virtual SelectionPromptBuilder BuildSelectionPrompt(string prompt)
        => new(prompt, _output);
    public virtual SelectionPromptBuilder<TValue> BuildSelectionPrompt<TValue>(string prompt, Func<TValue, object> selectKey)
        => new(prompt, selectKey, _output);
    public virtual SelectionPromptBuilder<TValue, TKey> BuildSelectionPrompt<TValue, TKey>(string prompt, Func<TValue, TKey> selectKey)
        where TKey : notnull
        => new(prompt, selectKey, _output);

    public virtual string ReadText()
        => ReadTextAsync().GetAwaiter().GetResult();

    public virtual Task<string> ReadTextAsync(CancellationToken ct = default) {
        var builder = new MultilinePromptBuilder(_output);
        return builder.ShowAsync(ct);
    }

    public virtual string Prompt(string prompt)
        => PromptAsync(prompt).GetAwaiter().GetResult();

    public virtual Task<string> PromptAsync(string prompt, CancellationToken ct = default) {
        _output.WriteLine($"[teal]{prompt}[/]");
        return ReadTextAsync(ct);
    }

    public virtual TValue Ask<TValue>(string prompt, params TValue[] choices)
        => AskAsync(prompt, choices).GetAwaiter().GetResult();
    public virtual TValue Ask<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices)
        => AskAsync(prompt, defaultChoice, otherChoices).GetAwaiter().GetResult();
    public virtual string Ask(string prompt, params string[] choices)
        => AskAsync(prompt, choices).GetAwaiter().GetResult();
    public virtual string Ask(string prompt, string defaultChoice, params string[] otherChoices)
        => AskAsync(prompt, defaultChoice, otherChoices).GetAwaiter().GetResult();
    public virtual TValue AskRequired<TValue>(string prompt, params TValue[] choices)
        => AskRequiredAsync(prompt, choices).GetAwaiter().GetResult();
    public virtual string AskRequired(string prompt, params string[] choices)
        => AskRequiredAsync(prompt, choices).GetAwaiter().GetResult();

    public virtual bool Confirm(string prompt, bool defaultChoice = true)
        => ConfirmAsync(prompt, defaultChoice).GetAwaiter().GetResult();

    public virtual Task<TValue> AskAsync<TValue>(string prompt, CancellationToken ct = default)
        => AskAsync<TValue>(prompt, [], ct);

    public virtual Task<TValue> AskAsync<TValue>(string prompt, TValue defaultChoice, CancellationToken ct = default)
        => AskAsync(prompt, defaultChoice, [], ct);
    public virtual Task<string> AskAsync(string prompt, CancellationToken ct = default)
        => AskAsync(prompt, [], ct);
    public virtual Task<string> AskAsync(string prompt, string defaultChoice, CancellationToken ct = default)
        => AskAsync(prompt, defaultChoice, [], ct);
    public virtual Task<TValue> AskRequiredAsync<TValue>(string prompt, CancellationToken ct = default)
        => AskRequiredAsync<TValue>(prompt, [], ct);
    public virtual Task<string> AskRequiredAsync(string prompt, CancellationToken ct = default)
        => AskRequiredAsync(prompt, [], ct);
    public virtual Task<TValue> AskAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        if (choices.Length > 0) builder.AddChoices(choices.AsEnumerable());
        return builder.ShowAsync(ct);
    }
    public virtual Task<TValue> AskRequiredAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        builder.ShowOptionalFlag();
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.ShowAsync(ct);
    }
    public virtual Task<TValue> AskAsync<TValue>(string prompt, TValue defaultChoice, TValue[] otherChoices, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<TValue>(prompt, _output);
        if (otherChoices.Length > 0) builder.AddChoices([defaultChoice, .. otherChoices]);
        builder.WithDefault(defaultChoice);
        return builder.ShowAsync(ct);
    }
    public virtual Task<string> AskAsync(string prompt, string[] choices, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.ShowAsync(ct);
    }
    public virtual Task<string> AskAsync(string prompt, string defaultChoice, string[] otherChoices, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        if (otherChoices.Length > 0) builder.AddChoices([defaultChoice, .. otherChoices]);
        builder.WithDefault(defaultChoice);
        return builder.ShowAsync(ct);
    }
    public virtual Task<string> AskRequiredAsync(string prompt, string[] choices, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<string>(prompt, _output);
        builder.ShowOptionalFlag();
        if (choices.Length > 0) builder.AddChoices(choices);
        return builder.ShowAsync(ct);
    }

    public virtual TValue? Select<TValue>(string prompt, Func<TValue, object> selectKey, params TValue[] choices)
        => SelectAsync(prompt, selectKey, choices).GetAwaiter().GetResult();
    public virtual TValue? Select<TValue>(string prompt, Func<TValue, object> selectKey, TValue defaultChoice, params TValue[] otherChoices)
        => SelectAsync(prompt, selectKey, defaultChoice, otherChoices).GetAwaiter().GetResult();
    public virtual TValue SelectRequired<TValue>(string prompt, Func<TValue, object> selectKey, params TValue[] choices)
        => SelectRequiredAsync(prompt, selectKey, choices).GetAwaiter().GetResult();
    public virtual string? Select(string prompt, params string[] choices)
        => SelectAsync(prompt, choices).GetAwaiter().GetResult();
    public virtual string? Select(string prompt, string defaultChoice, params string[] otherChoices)
        => SelectAsync(prompt, defaultChoice, otherChoices).GetAwaiter().GetResult();
    public virtual string SelectRequired(string prompt, params string[] choices)
        => SelectRequiredAsync(prompt, choices).GetAwaiter().GetResult();

    public virtual Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, CancellationToken ct = default)
        => SelectAsync(prompt, selectKey, [], ct);
    public virtual Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue defaultChoice, CancellationToken ct = default)
        => SelectAsync(prompt, selectKey, defaultChoice, [], ct);
    public virtual Task<TValue> SelectRequiredAsync<TValue>(string prompt, Func<TValue, object> selectKey, CancellationToken ct = default)
        => SelectRequiredAsync(prompt, selectKey, [], ct);
    public virtual Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue[] choices, CancellationToken ct = default) {
        var builder = new SelectionPromptBuilder<TValue>(prompt, selectKey, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return builder.ShowAsync(ct);
    }
    public virtual Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue defaultChoice, TValue[] otherChoices, CancellationToken ct = default) {
        var builder = new SelectionPromptBuilder<TValue>(prompt, selectKey, _output);
        builder.AddDefaultChoice(defaultChoice);
        if (otherChoices.Length < 1) throw new ArgumentException("At least two choices must be provided.", nameof(otherChoices));
        builder.AddChoices(otherChoices);
        return builder.ShowAsync(ct);
    }
    public virtual async Task<TValue> SelectRequiredAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue[] choices, CancellationToken ct = default) {
        var builder = new SelectionPromptBuilder<TValue>(prompt, selectKey, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return await builder.ShowAsync(ct) ?? throw new InvalidOperationException("A selected value is required.");
    }
    public virtual Task<string?> SelectAsync(string prompt, CancellationToken ct = default)
        => SelectAsync(prompt, [], ct);
    public virtual Task<string?> SelectAsync(string prompt, string defaultChoice, CancellationToken ct = default)
        => SelectAsync(prompt, defaultChoice, [], ct);
    public virtual Task<string> SelectRequiredAsync(string prompt, CancellationToken ct = default)
        => SelectRequiredAsync(prompt, [], ct);
    public virtual Task<string?> SelectAsync(string prompt, string[] choices, CancellationToken ct = default) {
        var builder = new SelectionPromptBuilder(prompt, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return builder.ShowAsync(ct);
    }
    public virtual Task<string?> SelectAsync(string prompt, string defaultChoice, string[] otherChoices, CancellationToken ct = default) {
        var builder = new SelectionPromptBuilder(prompt, _output);
        builder.AddDefaultChoice(defaultChoice);
        if (otherChoices.Length < 1) throw new ArgumentException("At least two choices must be provided.", nameof(otherChoices));
        builder.AddChoices(otherChoices);
        return builder.ShowAsync(ct);
    }
    public virtual async Task<string> SelectRequiredAsync(string prompt, string[] choices, CancellationToken ct = default) {
        var builder = new SelectionPromptBuilder(prompt, _output);
        if (choices.Length < 2) throw new ArgumentException("At least two choices must be provided.", nameof(choices));
        builder.AddChoices(choices);
        return await builder.ShowAsync(ct) ?? throw new InvalidOperationException("A selected value is required.");
    }

    public virtual Task<bool> ConfirmAsync(string prompt, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<bool>(prompt, _output);
        builder.AddChoices(true, false);
        builder.ConvertWith(value => value ? "y" : "n");
        return builder.ShowAsync(ct);
    }

    public virtual Task<bool> ConfirmAsync(string prompt, bool defaultChoice, CancellationToken ct = default) {
        var builder = new TextPromptBuilder<bool>(prompt, _output);
        builder.AddChoices(true, false);
        builder.ConvertWith(value => value ? "y" : "n");
        builder.WithDefault(defaultChoice);
        return builder.ShowAsync(ct);
    }
}
