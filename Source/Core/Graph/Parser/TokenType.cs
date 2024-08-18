namespace DotNetToolbox.Graph.Parser;

public enum TokenType {
    Indent,
    Identifier,
    Number,
    String,
    DateTime,
    Boolean,
    Range,
    Array,
    Label,
    If,
    Case,
    Else,
    Is,
    Otherwise,
    Exit,
    JumpTo,
    Id,
    Equal,
    NotEqual,
    GreaterThan,
    GreaterOrEqual,
    LessThan,
    LessOrEqual,
    And,
    Or,
    Not,
    Within,
    In,
    EndOfLine, // End of Line
    EndOfFile,  // End of File
    Error,  // Error token
}
