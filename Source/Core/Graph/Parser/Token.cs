namespace DotNetToolbox.Graph.Parser;

public record Token(TokenType Type, object Value, int Line, int Column);
