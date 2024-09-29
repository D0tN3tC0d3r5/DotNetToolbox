namespace DotNetToolbox.Environment;

public interface IInput {
    Encoding Encoding { get; set; }
    TextReader Reader { get; }

    int Read();
    ConsoleKeyInfo ReadKey(bool intercept = false);
    string? ReadLine();

    string ReadText();
    Task<string> ReadTextAsync(CancellationToken ct = default);

    string Prompt(string prompt);
    Task<string> PromptAsync(string prompt, CancellationToken ct = default);

    bool Confirm(string prompt, bool defaultChoice = true);
    Task<bool> ConfirmAsync(string prompt, CancellationToken ct = default);
    Task<bool> ConfirmAsync(string prompt, bool defaultChoice, CancellationToken ct = default);

    TValue Ask<TValue>(string prompt, params TValue[] choices);
    TValue Ask<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices);
    TValue AskRequired<TValue>(string prompt, params TValue[] choices);
    string Ask(string prompt, params string[] choices);
    string Ask(string prompt, string defaultChoice, params string[] otherChoices);
    string AskRequired(string prompt, params string[] choices);

    Task<TValue> AskAsync<TValue>(string prompt, CancellationToken ct = default);
    Task<TValue> AskAsync<TValue>(string prompt, TValue defaultChoice, CancellationToken ct = default);
    Task<TValue> AskAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default);
    Task<TValue> AskAsync<TValue>(string prompt, TValue defaultChoice, TValue[] otherChoices, CancellationToken ct = default);
    Task<TValue> AskRequiredAsync<TValue>(string prompt, CancellationToken ct = default);
    Task<TValue> AskRequiredAsync<TValue>(string prompt, TValue[] choices, CancellationToken ct = default);
    Task<string> AskAsync(string prompt, CancellationToken ct = default);
    Task<string> AskAsync(string prompt, string defaultChoice, CancellationToken ct = default);
    Task<string> AskAsync(string prompt, string[] choices, CancellationToken ct = default);
    Task<string> AskAsync(string prompt, string defaultChoice, string[] otherChoices, CancellationToken ct = default);
    Task<string> AskRequiredAsync(string prompt, CancellationToken ct = default);
    Task<string> AskRequiredAsync(string prompt, string[] choices, CancellationToken ct = default);

    TValue? Select<TValue>(string prompt, Func<TValue, object> selectKey, params TValue[] choices);
    TValue? Select<TValue>(string prompt, Func<TValue, object> selectKey, TValue defaultChoice, params TValue[] otherChoices);
    TValue SelectRequired<TValue>(string prompt, Func<TValue, object> selectKey, params TValue[] choices);
    string? Select(string prompt, params string[] choices);
    string? Select(string prompt, string defaultChoice, params string[] otherChoices);
    string SelectRequired(string prompt, params string[] choices);

    Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, CancellationToken ct = default);
    Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue defaultChoice, CancellationToken ct = default);
    Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue[] choices, CancellationToken ct = default);
    Task<TValue?> SelectAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue defaultChoice, TValue[] otherChoices, CancellationToken ct = default);
    Task<TValue> SelectRequiredAsync<TValue>(string prompt, Func<TValue, object> selectKey, CancellationToken ct = default);
    Task<TValue> SelectRequiredAsync<TValue>(string prompt, Func<TValue, object> selectKey, TValue[] choices, CancellationToken ct = default);
    Task<string?> SelectAsync(string prompt, string defaultChoice, CancellationToken ct = default);
    Task<string?> SelectAsync(string prompt, CancellationToken ct = default);
    Task<string?> SelectAsync(string prompt, string defaultChoice, string[] otherChoices, CancellationToken ct = default);
    Task<string?> SelectAsync(string prompt, string[] choices, CancellationToken ct = default);
    Task<string> SelectRequiredAsync(string prompt, CancellationToken ct = default);
    Task<string> SelectRequiredAsync(string prompt, string[] choices, CancellationToken ct = default);

    MultilinePromptBuilder BuildMultilinePrompt(string prompt);

    TextPromptBuilder<TValue> BuildTextPrompt<TValue>(string prompt);

    SelectionPromptBuilder BuildSelectionPrompt(string prompt);

    SelectionPromptBuilder<TValue> BuildSelectionPrompt<TValue>(string prompt, Func<TValue, object> selectKey);

    SelectionPromptBuilder<TValue, TKey> BuildSelectionPrompt<TValue, TKey>(string prompt, Func<TValue, TKey> selectKey)
        where TKey : notnull;
}
