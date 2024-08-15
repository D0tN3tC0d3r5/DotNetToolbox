namespace DotNetToolbox.Graph.Parser;

public record Token(TokenType Type, int Line = 0, int Column = 0, string? Value = null);

public record Word(string Text, int Column);
