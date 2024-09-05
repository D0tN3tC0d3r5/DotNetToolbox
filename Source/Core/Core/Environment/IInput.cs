namespace DotNetToolbox.Environment;

public interface IInput {
    Encoding Encoding { get; set; }
    TextReader Reader { get; }

    int Read();
    ConsoleKeyInfo ReadKey(bool intercept = false);
    string? ReadLine();
    string ReadText(ConsoleKey submitKey = ConsoleKey.Enter, ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None);

    bool Confirm(string prompt, bool defaultChoice = true);

    TValue Ask<TValue>(string prompt, params TValue[] choices);
    TValue Ask<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices);
    TValue AskRequired<TValue>(string prompt, params TValue[] choices);
    string Ask(string prompt, params string[] choices);
    string Ask(string prompt, string defaultChoice, params string[] otherChoices);
    string AskRequired(string prompt, params string[] choices);

    TValue Select<TValue>(string prompt, TValue defaultChoice, params TValue[] otherChoices)
        where TValue : notnull;
    TValue Select<TValue>(string prompt, params TValue[] choices)
        where TValue : notnull;
    TValue SelectRequired<TValue>(string prompt, params TValue[] choices)
        where TValue : notnull;
    string Select(string prompt, string defaultChoice, params string[] otherChoices);
    string Select(string prompt, params string[] choices);
    string SelectRequired(string prompt, params string[] choices);

    TextPromptBuilder<TValue> BuildTextPrompt<TValue>(string prompt);

    SelectionPromptBuilder<TValue> BuildSelectionPrompt<TValue>(string prompt)
        where TValue : notnull;
}
