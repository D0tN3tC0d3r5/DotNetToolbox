namespace DotNetToolbox.Graph.Parser;

public record Token(TokenType Type, int Line = 0, int Column = 0, string? Value = null) {
    public string ToSource() {
        var tokenValue = Value is null ? string.Empty : $": {Value}";
        var tokenPosition = $"({Line}, {Column})";
        return $"{Type}@{tokenPosition}{tokenValue}";
    }
}
