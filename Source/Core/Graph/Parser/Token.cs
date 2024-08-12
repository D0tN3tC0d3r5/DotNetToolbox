namespace DotNetToolbox.Graph.Parser;

public record Token(TokenType Type, int Line, int Column, string? Value = null);

public record Word(string Text, int Column);
