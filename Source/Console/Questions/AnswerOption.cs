namespace DotNetToolbox.ConsoleApplication.Questions;

public record AnswerOption {
    public string Value { get; }
    public string Text { get; }

    public AnswerOption(string value, string? text = null) {
        Value = IsNotNullOrWhiteSpace(value);
        Text = GetFormattedText();
        return;

        string GetFormattedText() {
            if (text is null) return $"{Value}";
            var index = IsNotNullOrWhiteSpace(text).IndexOf(Value, StringComparison.OrdinalIgnoreCase);
            return index == -1
                       ? $"[{Value}] {text}"
                       : $"{text[..index]}[{Value}]{text[(index + Value.Length)..]}";
        }
    }

    public bool Matches(string input)
        => Value.Equals(input, StringComparison.InvariantCultureIgnoreCase);
}
