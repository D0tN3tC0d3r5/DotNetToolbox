namespace DotNetToolbox.Graph.Parser;

public class WorkflowLexer {
    private readonly string[] _lines;
    private int _currentLine;
    private int _currentIndent;

    // START: Start
    // ACTION: DoSomething
    // IF: CheckCondition
    //   TRUE: TrueAction
    //   FALSE: FalseAction
    // WHEN: SelectPath
    //   CASE: Path1
    //     ACTION: Path1Action
    //   CASE: Path2
    //     ACTION: Path2Action
    // END: End

    public WorkflowLexer(string input) {
        _lines = input.Split('\n');
        _currentLine = 0;
        _currentIndent = 0;
    }

    public IEnumerable<Token> Tokenize() {
        while (_currentLine < _lines.Length) {
            var line = _lines[_currentLine].TrimEnd();
            var indent = CountIndent(line);
            line = line.Trim();

            if (string.IsNullOrWhiteSpace(line)) {
                _currentLine++;
                continue;
            }

            if (indent > _currentIndent) {
                yield return new Token(TokenType.Indent, "", _currentLine);
                _currentIndent = indent;
            }
            else if (indent < _currentIndent) {
                yield return new Token(TokenType.Dedent, "", _currentLine);
                _currentIndent = indent;
            }

            var parts = line.Split(':');
            if (parts.Length == 2) {
                yield return TokenizeKeyword(parts[0].Trim());
                yield return new Token(TokenType.Colon, ":", _currentLine);
                yield return new Token(TokenType.Identifier, parts[1].Trim(), _currentLine);
            }
            else {
                yield return TokenizeKeyword(line);
            }

            yield return new Token(TokenType.EOL, "\n", _currentLine);
            _currentLine++;
        }

        while (_currentIndent > 0) {
            yield return new Token(TokenType.Dedent, "", _currentLine);
            _currentIndent--;
        }
    }

    private Token TokenizeKeyword(string keyword) {
        return keyword.ToUpper() switch {
            "START" => new Token(TokenType.Start, keyword, _currentLine),
            "ACTION" => new Token(TokenType.Action, keyword, _currentLine),
            "IF" => new Token(TokenType.If, keyword, _currentLine),
            "WHEN" => new Token(TokenType.When, keyword, _currentLine),
            "TRUE" => new Token(TokenType.True, keyword, _currentLine),
            "FALSE" => new Token(TokenType.False, keyword, _currentLine),
            "CASE" => new Token(TokenType.Case, keyword, _currentLine),
            "END" => new Token(TokenType.End, keyword, _currentLine),
            _ => new Token(TokenType.Identifier, keyword, _currentLine)
        };
    }

    private int CountIndent(string line) {
        int count = 0;
        foreach (char c in line) {
            if (c == ' ') count++;
            else if (c == '\t') count += 4;
            else break;
        }
        return count / 2;
    }
}
