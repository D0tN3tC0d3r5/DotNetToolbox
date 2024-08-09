namespace DotNetToolbox.Graph.Parser;

public enum TokenType {
    Start,
    Action,
    If,
    When,
    True,
    False,
    Case,
    End,
    Identifier,
    Colon,
    Indent,
    Dedent,
    EOL
}
