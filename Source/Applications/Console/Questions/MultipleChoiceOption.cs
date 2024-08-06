namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoiceOption(int index, string value, string text, string? alias = null)
    : MultipleChoiceOption<string>(index, value, text, alias) {
    public MultipleChoiceOption(int index, string value, string text, bool displayOrder)
        : this(index, value, text, displayOrder ? index.ToString() : null!) {
    }

    public MultipleChoiceOption(int index, string value, string? alias = null)
        : this(index, value, value, alias) {
    }

    public MultipleChoiceOption(int index, string value, bool displayOrder)
        : this(index, value, value, displayOrder ? index.ToString() : null!) {
    }
}

public class MultipleChoiceOption<TValue>(int index, TValue value, string text, string? alias = null) {
    public MultipleChoiceOption(int index, TValue value, string text, bool displayOrder)
        : this(index, value, text, displayOrder ? index.ToString() : null) {
    }

    private static string FormatDefaultDisplay(string text, string? alias = null) {
        var sb = new StringBuilder();
        var pos = IsNotNullOrWhiteSpace(text).IndexOf(alias ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        if ((alias is not null) && pos != -1) sb.Append(text[..pos]).Append($"[{alias}]").Append(text[(pos + alias.Length)..]);
        else if (alias is not null) sb.Append($"[{alias}] ").Append(text);
        else sb.Append(text);
        return sb.ToString();
    }

    public int Index { get; } = index;
    public TValue Value { get; } = value;
    public string Display { get; } = FormatDefaultDisplay(text, alias);

    public bool Matches(string input)
        => alias?.Equals(input, StringComparison.OrdinalIgnoreCase)
        ?? text.Equals(input, StringComparison.OrdinalIgnoreCase);
}
