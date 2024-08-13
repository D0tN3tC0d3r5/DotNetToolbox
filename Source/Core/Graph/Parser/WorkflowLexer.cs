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
    private const uint _indentSize = 2;
    private int _currentLine;
    private int _currentColumn;

    private WorkflowLexer() {
    }

    public static IEnumerable<Token> Tokenize(string input) {
        var lexer = new WorkflowLexer();
        return lexer.Process(input);
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

    private IEnumerable<Token> Process(string input) {
        input = input.Replace("\r\n", "\n") // windows EOL
                     .Replace("\r", "\n");  // Mac OS EOL
        var lines = input.Split('\n').Select(l => l.TrimEnd()).ToArray();
        _currentLine = 1;
        var length = 0;
        foreach (var line in lines) {
            _currentColumn = 1;
            var code = StripComments(line);
            length += code.Length;
            foreach (var token in ProcessLine(code))
                yield return token;
            _currentLine++;
        }

        yield return new Token(TokenType.EOF, _currentLine - 1, length);
    }

    private static string StripComments(string line) {
        var commentIndex = line.IndexOf('#');
        return commentIndex > -1 ? line[..commentIndex].TrimEnd() : line.TrimEnd();
    }

    private IEnumerable<Token> ProcessLine(string line) {
        foreach (var indent in ProcessIndent(line)) {
            yield return indent;
        }
        foreach (var token in SplitLine(line)) {
            yield return TokenizeWord(token);
        }

        if (line.Length > 0) { // remove empty lines
            yield return new Token(TokenType.EOL, _currentLine, line.Length);
        }
        else {
            _currentLine--;
        }
    }

    private IEnumerable<Word> SplitLine(string line) {
        var type = LineSectionType.None;
        var wordBuilder = new StringBuilder();
        var nextWordStart = _currentColumn;

        var nextChar = Pop(line);
        while (true) {
            switch (nextChar) {
                case '\0':
                    _currentColumn--;
                    if (GetWord() is { } w) yield return w;
                    yield break;
                case '(' when type is LineSectionType.None:
                case ':' when type is LineSectionType.None:
                case '"' when type is LineSectionType.None:
                case '`' when type is LineSectionType.None:
                case '{' when type is LineSectionType.None:
                case '[' when type is LineSectionType.None:
                case '|' when type is LineSectionType.None:
                    if (GetWord() is { } w1) yield return w1;
                    wordBuilder.Append(nextChar);
                    nextWordStart--;
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

        Word? GetWord() {
            var wordStart = nextWordStart;
            nextWordStart = _currentColumn;
            if (wordBuilder.Length == 0) return null;
            var result = new Word(wordBuilder.ToString(), wordStart);
            wordBuilder.Clear();
            return result;
        }
    }

    private Token TokenizeWord(Word word) {
        var (type, value) = word.Text switch {
            "==" => (TokenType.Equal, word.Text),
            "!=" => (TokenType.NotEqual, word.Text),
            ">" => (TokenType.GreaterThan, word.Text),
            ">=" => (TokenType.GreaterOrEqual, word.Text),
            "<" => (TokenType.LessThan, word.Text),
            "<=" => (TokenType.LessOrEqual, word.Text),
            "AND" => (TokenType.And, word.Text),
            "OR" => (TokenType.Or, word.Text),
            "NOT" => (TokenType.Not, word.Text),
            "WITHIN" => (TokenType.Within, word.Text),
            "IN" => (TokenType.In, word.Text),
            "IF" => (TokenType.If, word.Text),
            "CASE" => (TokenType.Case, word.Text),
            "THEN" => (TokenType.Then, word.Text),
            "ELSE" => (TokenType.Else, word.Text),
            "IS" => (TokenType.Is, word.Text),
            "OTHERWISE" => (TokenType.Otherwise, word.Text),
            "EXIT" => (TokenType.Exit, word.Text),
            "GOTO" => (TokenType.JumpTo, word.Text),
            var w when w.StartsWith('`') && w.EndsWith('`') => (TokenType.Label, w.Trim('`', '`')),
            var w when w.StartsWith(':') && w.EndsWith(':') => (TokenType.Tag, w.Trim(':')),
            var w when w.StartsWith('"') && w.EndsWith('"') => (TokenType.String, w.Trim('"')),
            var w when w.StartsWith('(') && w.EndsWith(')') && DateTime.TryParse(w.Trim('(', ')'), out var dt) => (TokenType.DateTime, $"{dt:s}.{dt:fffffff}"),
            var w when (w.StartsWith('[') || w.StartsWith('|')) && (w.EndsWith(']') || w.EndsWith('|')) => (TokenType.Range, w),
            var w when w.StartsWith('{') && w.EndsWith('}') => (TokenType.Array, w.Trim('{', '}')),
            var w when int.TryParse(w, out var i) => (TokenType.Number, $"{i}"),
            var w when decimal.TryParse(w, out var d) => (TokenType.Number, $"{d}"),
            var w when bool.TryParse(w, out var b) => (TokenType.Boolean, $"{b}"),
            _ => (TokenType.Identifier, word.Text),
        };
        return new Token(type, _currentLine, word.Column, value);
    }

    private IEnumerable<Token> ProcessIndent(string line) {
        var nextChar = Peek(line);
        var count = 0;
        while (char.IsWhiteSpace(nextChar)) {
            count++;
            _currentColumn++;
            if (nextChar is '\t' || count >= _indentSize) {
                yield return new Token(TokenType.Indent, _currentLine);
                count = 0;
            }
            nextChar = Peek(line);
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
