namespace DotNetToolbox.Environment;

public interface IInput {
    Encoding Encoding { get; set; }
    TextReader Reader { get; }

    int Read();
    ConsoleKeyInfo ReadKey(bool intercept = false);
    string? ReadLine();
    string ReadText(ConsoleKey submitKey = ConsoleKey.Enter, ConsoleModifiers submitKeyModifiers = ConsoleModifiers.None);

    TextPromptBuilder<TValue> TextPrompt<TValue>(string prompt);
    TextPromptBuilder<string> TextPrompt(string prompt);

    TValue Ask<TValue>(string prompt, TValue defaultChoice);
    TValue Ask<TValue>(string prompt);
    string Ask(string prompt);

    bool Confirm(string prompt, bool defaultChoice = true);

    SelectionPromptBuilder<TValue> SelectionPrompt<TValue>(string prompt)
        where TValue : notnull;
    SelectionPromptBuilder<string> SelectionPrompt(string prompt);

    TValue Select<TValue>(string prompt, TValue defaultChoice)
        where TValue : notnull;
    TValue Select<TValue>(string prompt)
        where TValue : notnull;
    string Select(string prompt);
}
