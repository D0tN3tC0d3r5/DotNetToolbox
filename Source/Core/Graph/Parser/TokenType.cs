﻿namespace DotNetToolbox.Graph.Parser;

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
    Then,
    Else,
    Is,
    Otherwise,
    Exit,
    JumpTo,
    Tag,
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
