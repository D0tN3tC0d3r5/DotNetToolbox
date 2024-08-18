using Word = (string Text, int Column);

namespace DotNetToolbox.Graph.Parser;

// # this is the line 1 of a comment
// # this is the line 2 of a comment
// Initialize
// DoSomething :Label1: `This is a node description` # this is a comment
//   IF CheckCondition :Label2: `Condition description`
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

        yield return new(TokenType.EndOfFile, _currentLine - 1, length);
    }

    private static string StripComments(string line) {
        var commentIndex = line.IndexOf('#');
        return commentIndex > -1 ? line[..commentIndex].TrimEnd() : line.TrimEnd();
    }

    private IEnumerable<Token> ProcessLine(string line) {
        foreach (var indent in ProcessIndent(line))
            yield return indent;

        foreach (var token in SplitLine(line))
            yield return TokenizeWord(token);

        if (line.Length == 0) {
            _currentLine--;
            yield break;
        }

        yield return new(TokenType.EndOfLine, _currentLine, line.Length);
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
                case '\'' when type is LineSectionType.None:
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
                        '\'' => LineSectionType.DateTime,
                        ':' => LineSectionType.Tag,
                        '"' => LineSectionType.String,
                        '`' => LineSectionType.Description,
                        '{' => LineSectionType.Array,
                        _ => LineSectionType.Range,
                    };
                    break;
                case '\'' when type is LineSectionType.DateTime:
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
        (var type, var value) = word.Text switch {
            "==" => (TokenType.Equal, null),
            "!=" => (TokenType.NotEqual, null),
            ">" => (TokenType.GreaterThan, null),
            ">=" => (TokenType.GreaterOrEqual, null),
            "<" => (TokenType.LessThan, null),
            "<=" => (TokenType.LessOrEqual, null),
            "AND" => (TokenType.And, null),
            "OR" => (TokenType.Or, null),
            "NOT" => (TokenType.Not, null),
            "IN" => (TokenType.In, null),
            "WITHIN" => (TokenType.Within, null),
            "IF" => (TokenType.If, null),
            "ELSE" => (TokenType.Else, null),
            "CASE" => (TokenType.Case, null),
            "IS" => (TokenType.Is, null),
            "OTHERWISE" => (TokenType.Otherwise, null),
            "EXIT" => (TokenType.Exit, null),
            "GOTO" => (TokenType.JumpTo, null),
            ['`', .. var w, '`'] => (TokenType.Label, w),
            [':', .. var w, ':'] => (Tag: TokenType.Id, w),
            ['"', .. var w, '"'] => (TokenType.String, w),
            ['[' or '|', .., '|' or ']'] => (TokenType.Range, word.Text),
            ['{', .. var w, '}'] => (TokenType.Array, string.Join(",", w.Trim().Split(',').Select(a => a.Trim()))),
            ['\'', .. var w, '\''] when DateTime.TryParse(w, out var dt) => (TokenType.DateTime, $"{dt:s}.{dt:fffffff}"),
            var w when int.TryParse(w, out var i) => (TokenType.Number, $"{i}"),
            var w when decimal.TryParse(w, out var d) => (TokenType.Number, $"{d}"),
            var w when bool.TryParse(w, out var b) => (TokenType.Boolean, $"{b}"),
            var w when IsValidIdentifier(w) => (TokenType.Identifier, w),
            _ => (TokenType.Error, GetIdentifierError(word.Text)),
        };
        return new(type, _currentLine, word.Column, value);
    }

    private static bool IsValidIdentifier(string token)
        => !string.IsNullOrEmpty(token)
        && token.Length <= 64
        && char.IsLetter(token[0])
        && token.All(c => char.IsLetterOrDigit(c) || c == '_');

    private static string GetIdentifierError(string token)
        => token switch {
            _ when string.IsNullOrEmpty(token) => "Identifier cannot be empty.",
            _ when token.Length > 64 => "Identifier must not exceed 64 characters.",
            _ when char.IsNumber(token[0]) => "Identifier must start with a letter.",
            _ when token[0] == '_' => "Identifier must start with a letter.",
            _ when char.IsLetter(token[0]) && !token.All(c => char.IsLetterOrDigit(c) || c == '_') => "Identifier can only contain letters, numbers, and underscores.",
            _ => "Invalid token.",
        };

    private IEnumerable<Token> ProcessIndent(string line) {
        var nextChar = Peek(line);
        var count = 0;
        while (char.IsWhiteSpace(nextChar)) {
            count++;
            _currentColumn++;
            if (nextChar is '\t' || count >= _indentSize) {
                yield return new(TokenType.Indent, _currentLine);
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

    private Token CreateErrorToken(string errorMessage)
        => new(TokenType.Error, _currentLine, _currentColumn, errorMessage);
}
