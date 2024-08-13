namespace DotNetToolbox.Graph.Parser;

// # this is the line 1 of a comment
// # this is the line 2 of a comment
// Initialize
// DoSomething :Label1: `This is a node description` # this is a comment
//   IF CheckCondition :Label2: `Condiftion description`
//     TrueAction1 `Update X`
//     TrueAction2
//     EXIT 1 `Alternative Exit`
//   ELSE
//     FalseAction # this is another comment
// CASE SelectPath `Path1 description`
//   IS "Path 1"
//     Path1Action
//     GOTO 0
//   IS "Path 2" `Path2 description`
//     Path2Action1 :Label3:
//     Path2Action2
//     IF CheckAnotherCondition
//       Path2Action2TrueAction `Update X`
//   OTHERWISE
//     GOTO Label2
// DoSomethingElse
// DoSomething
// EXIT

public sealed class WorkflowLexer {
    private const int _indentSize = 2;
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

        var length = 0;
        foreach (var line in lines) {
            var code = StripComments(line);
            length += code.Length;
            foreach (var token in ProcessLine(code))
                yield return token;
            _currentLine++;
        }

        while (_currentIndent > 0) {
            _currentIndent--;
            yield return new Token(TokenType.Dedent, _currentLine - 1, _currentIndent * 2);
        }
        yield return new Token(TokenType.EOF, _currentLine - 1, length);
    }

    private static string StripComments(string line) {
        var commentIndex = line.IndexOf('#');
        return commentIndex > -1 ? line[..commentIndex] : line;
    }

    private IEnumerable<Token> ProcessLine(string line) {
        ProcessIndent(line);
        foreach (var token in SplitLine(line)) {
            yield return TokenizeWord(token);
        }

        yield return new Token(TokenType.EOL, _currentLine, line.Length);
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
        var type = LineSectionType.None;
        var wordBuilder = new StringBuilder();
        var wordStart = _currentColumn;
        var exit = false;

        var nextChar = Pop(line);
        while (!exit) {
            switch (nextChar) {
                case '\0':
                    exit = true;
                    break;
                case '(' when type is LineSectionType.None:
                case ':' when type is LineSectionType.None:
                case '"' when type is LineSectionType.None:
                case '`' when type is LineSectionType.None:
                case '{' when type is LineSectionType.None:
                case '[' when type is LineSectionType.None:
                case '|' when type is LineSectionType.None:
                    if (GetWord() is { } w1) yield return w1;
                    wordBuilder.Append(nextChar);
                    type = nextChar switch {
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
                    wordBuilder.Append(nextChar);
                    if (GetWord() is { } w2) yield return w2;
                    type = LineSectionType.None;
                    break;
                case var _ when char.IsWhiteSpace(nextChar) && type is LineSectionType.None:
                    if (GetWord() is { } w3) yield return w3;
                    break;
                default:
                    wordBuilder.Append(nextChar);
                    break;
            }
            nextChar = Pop(line);
        }
        if (wordBuilder.Length > 0) {
            yield return new(wordBuilder.ToString(), wordStart);
            wordBuilder.Clear();
        }

        Word? GetWord() {
            if (wordBuilder.Length == 0) return null;
            var result = new Word(wordBuilder.ToString(), wordStart);
            wordBuilder.Clear();
            wordStart = _currentColumn;
            return result;
        }
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

    private IEnumerable<Token> ProcessIndent(string line) {
        var count = 0;
        var indentLevel = 0;
        var nextChar = Pop(line);
        while (char.IsWhiteSpace(Pop(line))) {
            if (nextChar is not ' ')
                throw new Exception("Only spaces are allowed for indentation");
            count++;
            if (count >= _indentSize) {
                indentLevel++;
                count = 0;
            }
            nextChar = Pop(line);
        }
        while (indentLevel < _currentIndent) {
            _currentIndent--;
            yield return new Token(TokenType.Dedent, _currentLine - 1, _currentIndent * 2);
        }

        _currentIndent = 0;
        while (_currentIndent < indentLevel) {
            _currentIndent++;
            yield return new Token(TokenType.Indent, _currentLine, _currentIndent * 2);
        }
    }

    private char Peek(string line)
        => _currentColumn <= line.Length ? line[_currentColumn - 1] : '\0';

    private char Pop(string line) {
        var c = Peek(line);
        _currentColumn++;
        return c;
    }
}
