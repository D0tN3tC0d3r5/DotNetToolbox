namespace DotNetToolbox.Graph.Parser;

public enum TokenType {
    Action,
    If,
    When,
    Then,
    Else,
    Is,
    Otherwise,
    Exit,
    Goto,
    Identifier,
    Number,
    Label,
    Description,
    String,
    Indent,
    Dedent,
    EOL
}
