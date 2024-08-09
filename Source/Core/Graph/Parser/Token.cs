namespace DotNetToolbox.Graph.Parser;

public class Token {
    public TokenType Type { get; }
    public string Value { get; }
    public int LineNumber { get; }

    public Token(TokenType type, string value, int lineNumber) {
        Type = type;
        Value = value;
        LineNumber = lineNumber;
    }
}