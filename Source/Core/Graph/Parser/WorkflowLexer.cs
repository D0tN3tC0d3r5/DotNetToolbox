namespace DotNetToolbox.Graph.Parser;

// # this is the line 1 of a comment
// # this is the line 2 of a comment
// Initialize
// :Label1
// DoSomething `This is a node description` # this is a comment
//   :Label2
//   IF CheckCondition `Condiftion description`
//     THEN
//       TrueAction1 `Update X`
//       TrueAction2
//       EXIT 1 `Alternative Exit`
//     ELSE
//       FalseAction # this is another comment
// CASE SelectPath  `Path1 description`
//   IS "Path 1"
//     Path1Action
//     GOTO 0
//   IS "Path 2" `Path2 description`
//     :Label3
//     Path2Action1
//     Path2Action2
//     IF CheckAnotherCondition
//       THEN
//         Path2Action2TrueAction `Update X`
//   OTHERWISE
//     GOTO Label2
// DoSomethingElse
// DoSomething
// EXIT

public class WorkflowLexer {
    private int _currentLine;
    private int _currentColumn;
    private int _currentIndent;

    private WorkflowLexer() {
    }

    public static IEnumerable<Token> Tokenize(string input) {
        var lexer = new WorkflowLexer();
        return lexer.Process(input);
    }

    private IEnumerable<Token> Process(string input) {
        input = input.Replace("\r\n", "\n") // windows EOL
                     .Replace("\r", "\n");  // Mac OS EOL
        var lines = input.Split('\n').Select(l => l.TrimEnd()).ToArray();
        _currentLine = 1;
        _currentColumn = 1;
        _currentIndent = 0;

        var length = lines.Sum(l => l.Length);
        var tokens = new List<Token>();
        foreach (var line in lines) {
            tokens.AddRange(ProcessLine(line));
            _currentLine++;
        }
        var dedentCount = 0;
        while (_currentIndent > 0) {
            dedentCount++;
            _currentIndent--;
        }
        if (dedentCount > 0)
            tokens.Add(new Token(TokenType.Dedent, _currentLine - 1, _currentColumn, $"{dedentCount}"));
        tokens.Add(new Token(TokenType.EOF, _currentLine - 1, _currentColumn, $"{length}"));
        return tokens;
    }

    private IEnumerable<Token> ProcessLine(string line) {
        (var indentLevel, var startColumn) = GetIndent(line);
        var dedentCount = 0;
        while (indentLevel < _currentIndent) {
            dedentCount++;
            _currentIndent--;
        }
        if (dedentCount > 0)
            yield return new Token(TokenType.Dedent, _currentLine - 1, _currentColumn, $"{dedentCount}");
        _currentColumn = startColumn;
        var lineLength = line.Length;
        if (indentLevel != 0) {
            yield return new Token(TokenType.Indent, _currentLine, 1, $"{indentLevel}");
            _currentIndent = indentLevel;
            line = line[(_currentColumn - 1)..];
        }

        foreach (var token in SplitLine(line)) {
            yield return TokenizeWord(token);
        }

        yield return new Token(TokenType.EOL, _currentLine, lineLength + 1, $"{lineLength}");
    }

    private enum LineSectionType {
        None,
        Tag,
        String,
        Description,
        DateTime,
        Array,
        Range,
    }

    private IEnumerable<Word> SplitLine(string line) {
        var tokens = new List<Word>();
        var currentToken = new StringBuilder();
        var type = LineSectionType.None;
        var tokenStart = _currentColumn;

        for (var i = 0; i < line.Length; i++) {
            switch (line[i]) {
                case '#' when type is LineSectionType.None:
                    if (currentToken.Length > 0) {
                        tokens.Add(new(currentToken.ToString(), tokenStart));
                        currentToken.Clear();
                    }
                    i = line.Length;
                    break;
                case '(' when type is LineSectionType.None:
                case ':' when type is LineSectionType.None:
                case '"' when type is LineSectionType.None:
                case '`' when type is LineSectionType.None:
                case '{' when type is LineSectionType.None:
                case '[' when type is LineSectionType.None:
                case '|' when type is LineSectionType.None:
                    if (currentToken.Length > 0) {
                        tokens.Add(new(currentToken.ToString(), tokenStart));
                        tokenStart += currentToken.Length;
                        currentToken.Clear();
                    }
                    currentToken.Append(line[i]);
                    type = line[i] switch {
                        '(' => LineSectionType.DateTime,
                        ':' => LineSectionType.Tag,
                        '"' => LineSectionType.String,
                        '`' => LineSectionType.Description,
                        '{' => LineSectionType.Array,
                        _ => LineSectionType.Range,
                    };
                    break;
                case ')' when type is LineSectionType.DateTime:
                case ':' when type is LineSectionType.Tag:
                case '"' when type is LineSectionType.String:
                case '`' when type is LineSectionType.Description:
                case '}' when type is LineSectionType.Array:
                case ']' when type is LineSectionType.Range:
                case '|' when type is LineSectionType.Range:
                    currentToken.Append(line[i]);
                    tokens.Add(new(currentToken.ToString(), tokenStart));
                    tokenStart += currentToken.Length;
                    type = LineSectionType.None;
                    currentToken.Clear();
                    break;
                default:
                    if (char.IsWhiteSpace(line[i]) && type is LineSectionType.None) {
                        if (currentToken.Length > 0) {
                            tokens.Add(new(currentToken.ToString(), tokenStart));
                            tokenStart += currentToken.Length;
                            currentToken.Clear();
                        }
                        tokenStart++;
                    }
                    else {
                        currentToken.Append(line[i]);
                    }
                    break;
            }
        }

        if (currentToken.Length > 0) {
            tokens.Add(new(currentToken.ToString(), tokenStart));
        }

        return tokens;
    }

    private Token TokenizeWord(Word word) {
        var tokenStart = word.Column;
        _currentColumn = word.Column + word.Text.Length;

        return word.Text switch {
            var w when w.StartsWith('`') && w.EndsWith('`')
                => new Token(TokenType.Label, _currentLine, tokenStart, w.Trim('`', '`')),
            var w when w.StartsWith(':') && w.EndsWith(':')
                => new Token(TokenType.Tag, _currentLine, tokenStart, w.Trim(':')),
            var w when w.StartsWith('"') && w.EndsWith('"')
                => new Token(TokenType.String, _currentLine, tokenStart, w.Trim('"')),
            var w when w.StartsWith('(') && w.EndsWith(')') && DateTime.TryParse(w.Trim('(', ')'), out var dt)
                => new Token(TokenType.DateTime, _currentLine, tokenStart, w.Trim('(', ')')),
            var w when (w.StartsWith('[') || w.StartsWith('|')) && (w.EndsWith(']') || w.EndsWith('|'))
                => new Token(TokenType.Range, _currentLine, tokenStart, w),
            var w when w.StartsWith('{') && w.EndsWith('}')
                => new Token(TokenType.Array, _currentLine, tokenStart, w.Trim('{', '}')),
            var w when int.TryParse(w, out var i)
                => new Token(TokenType.Number, _currentLine, tokenStart, $"{i}"),
            var w when decimal.TryParse(w, out var d)
                => new Token(TokenType.Number, _currentLine, tokenStart, $"{d}"),
            var w when bool.TryParse(w, out var b)
                => new Token(TokenType.Boolean, _currentLine, tokenStart, $"{b}"),
            "==" => new Token(TokenType.Equal, _currentLine, tokenStart, word.Text),
            "!=" => new Token(TokenType.NotEqual, _currentLine, tokenStart, word.Text),
            ">" => new Token(TokenType.GreaterThan, _currentLine, tokenStart, word.Text),
            ">=" => new Token(TokenType.GreaterOrEqual, _currentLine, tokenStart, word.Text),
            "<" => new Token(TokenType.LessThan, _currentLine, tokenStart, word.Text),
            "<=" => new Token(TokenType.LessOrEqual, _currentLine, tokenStart, word.Text),
            "AND" => new Token(TokenType.And, _currentLine, tokenStart, word.Text),
            "OR" => new Token(TokenType.Or, _currentLine, tokenStart, word.Text),
            "NOT" => new Token(TokenType.Not, _currentLine, tokenStart, word.Text),
            "WITHIN" => new Token(TokenType.Within, _currentLine, tokenStart, word.Text),
            "IN" => new Token(TokenType.In, _currentLine, tokenStart, word.Text),
            "TRUE" => new Token(TokenType.Boolean, _currentLine, tokenStart, word.Text),
            "FALSE" => new Token(TokenType.Boolean, _currentLine, tokenStart, word.Text),
            "IF" => new Token(TokenType.If, _currentLine, tokenStart, word.Text),
            "CASE" => new Token(TokenType.Case, _currentLine, tokenStart, word.Text),
            "THEN" => new Token(TokenType.Then, _currentLine, tokenStart, word.Text),
            "ELSE" => new Token(TokenType.Else, _currentLine, tokenStart, word.Text),
            "IS" => new Token(TokenType.Is, _currentLine, tokenStart, word.Text),
            "OTHERWISE" => new Token(TokenType.Otherwise, _currentLine, tokenStart, word.Text),
            "EXIT" => new Token(TokenType.Exit, _currentLine, tokenStart, word.Text),
            "GOTO" => new Token(TokenType.JumpTo, _currentLine, tokenStart, word.Text),
            _ => new Token(TokenType.Identifier, _currentLine, tokenStart, word.Text),
        };
    }

    private static (int, int) GetIndent(string line) {
        var offset = 0;
        var position = 1;
        var exit = false;
        if (line.Length == 0) return (0, 1);
        foreach (var c in line) {
            switch (c) {
                case '\t':
                    position++;
                    offset += 2;
                    break;
                case ' ':
                    position++;
                    offset++;
                    break;
                default:
                    exit = true;
                    break;
            }
            if (exit) break;
        }
        if (line[position - 1] == ' ') position++;
        return (offset / 2, position);
    }
}
