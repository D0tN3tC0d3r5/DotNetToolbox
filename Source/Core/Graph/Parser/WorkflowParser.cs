namespace DotNetToolbox.Graph.Parser;

public sealed class WorkflowParser {
    private readonly IEnumerator<Token> _tokens;
    private Token _currentToken;
    private readonly List<ValidationError> _errors = [];
    private readonly Dictionary<INode, Token> _nodeMap = [];
    private readonly IServiceProvider _services;

    private WorkflowParser(IEnumerable<Token> tokens, IServiceProvider services) {
        _tokens = tokens.GetEnumerator();
        NextToken();
        _services = services;
    }

    public static Result<INode?> Parse(IEnumerable<Token> tokens, IServiceProvider services) {
        var parser = new WorkflowParser(tokens, services);
        var node = parser.Process();
        var result = Success<INode?>(node);
        result += parser.SetJumps();
        return parser._errors.Aggregate(result, (current, error) => current + error);
    }

    private Result SetJumps() {
        var result = Success();
        foreach (var (node, token) in _nodeMap) {
            if (node is JumpNode jumpNode) {
                var targetNode = _nodeMap.Keys.FirstOrDefault(n => n.Tag == jumpNode.TargetTag);
                if (targetNode == null) {
                    result += new ValidationError($"Jump target '{jumpNode.TargetTag}' not found.", $"[{token.Line}, {token.Column}]: {token.Type}.");
                }
                else {
                    jumpNode.Next = targetNode;
                }
            }
        }
        return result;
    }

    private INode? Process(string? id = null) {
        var builder = new WorkflowBuilder(_services, id, null);
        ParseStatements(builder);
        var result = builder.Build();
        if (result.IsSuccess)
            return result.Value;
        throw new ValidationException(result.Errors);
    }

    private void ParseStatements(WorkflowBuilder builder) {
        var indentColumn = _currentToken.Column;
        while (_currentToken.Type is not TokenType.EndOfFile && _currentToken.Column >= indentColumn)
            ParseStatement(builder);
    }

    private void ParseStatement(WorkflowBuilder builder) {
        switch (_currentToken.Type) {
            case TokenType.If:
                ParseIf(builder);
                break;
            case TokenType.Case:
                ParseCase(builder);
                break;
            case TokenType.Exit:
                ParseExit(builder);
                break;
            case TokenType.JumpTo:
                ParseJumpTo(builder);
                break;
            case TokenType.Identifier:
                ParseIdentifier(builder);
                break;
            case TokenType.Error: // acts as an identifier.
                AddError(_currentToken.Value);
                NextToken();
                break;
            case TokenType.EndOfLine: // acts as an identifier.
                NextToken();
                break;
            default:
                AddError("Unexpected token.");
                NextToken();
                break;
        }
    }

    private void ParseIdentifier(IWorkflowBuilder builder) {
        var name = GetValueFrom(TokenType.Identifier);
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label) ?? name;

        try {
            builder.Do(tag!, BuildAction(name), label);
        }
        catch (Exception ex) {
            AddError($"Error creating action node: {ex.Message}");
        }

        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
    }

    private void ParseIf(WorkflowBuilder builder) {
        Ensure(TokenType.If);
        var predicate = ParsePredicate();
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);

        try {
            builder.If(tag!,
                       BuildPredicate(predicate),
                       b => b.IsTrue(t => ParseThen((WorkflowBuilder)t))
                             .IsFalse(f => ParseElse((WorkflowBuilder)f)),
                       label);
        }
        catch (Exception ex) {
            AddError(ex.Message);
        }
    }

    private string ParsePredicate() {
        var condition = new StringBuilder();
        while (_currentToken.Type is not TokenType.Tag and not TokenType.Label and not TokenType.EndOfLine) {
            condition.Append(_currentToken.Value);
            condition.Append(' ');
            NextToken();
        }
        return condition.ToString().Trim();
    }

    private void ParseThen(WorkflowBuilder builder) {
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
        ParseStatements(builder);
    }

    private void ParseElse(WorkflowBuilder builder) {
        if (!Has(TokenType.Else))
            return;
        Ensure(TokenType.Else);
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
        ParseStatements(builder);
    }

    private void ParseCase(WorkflowBuilder builder) {
        Ensure(TokenType.Case);
        var selector = GetValueFrom(TokenType.Identifier);
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);

        try {
            builder.Case(tag!,
                         BuildSelector(selector),
                         b => ParseCaseOptions((WorkflowBuilder)b),
                         label);
        }
        catch (Exception ex) {
            AddError($"Error creating branching node: {ex.Message}");
        }
    }

    private void ParseCaseOptions(WorkflowBuilder builder) {
        var count = 0;
        while (TryParseCaseOption(builder)) count++;
        if (count == 0) Forbid(TokenType.Otherwise);
        ParseOtherwise(builder);
    }

    private bool TryParseCaseOption(WorkflowBuilder builder) {
        if (!Has(TokenType.Is)) return false;
        Ensure(TokenType.Is);
        var caseValue = GetValueFrom(TokenType.String);
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
        builder.Is(caseValue, b => ParseStatements((WorkflowBuilder)b));
        return true;
    }

    private void ParseOtherwise(WorkflowBuilder branches) {
        if (!Has(TokenType.Otherwise)) return;
        Ensure(TokenType.Otherwise);
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
        branches.Otherwise(b => ParseStatements((WorkflowBuilder)b));
        Forbid(TokenType.Otherwise);
    }

    private void ParseExit(IWorkflowBuilder builder) {
        Ensure(TokenType.Exit);
        var number = GetValueOrDefaultFrom(TokenType.Number) ?? "0"; // if number not found default to 0
        var exitCode = int.Parse(number);
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        builder.Exit(tag, exitCode, label);
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
    }

    private void ParseJumpTo(IWorkflowBuilder builder) {
        Ensure(TokenType.JumpTo);
        var target = GetValueFrom(TokenType.Identifier);
        builder.JumpTo(target);
        Ensure(TokenType.EndOfLine);
        AllowMany(TokenType.Indent);
    }

    private Action<Context> BuildAction(string action) {
        // fetch or build the action expression;
        return Result;

        static void Result(Context _) { }
    }
    private Func<Context, bool> BuildPredicate(string condition) {
        // fetch or build the predicate expression;
        return Result;

        static bool Result(Context _) => true;
    }

    private Func<Context, string> BuildSelector(string selector) {
        // fetch or build the selector expression;
        return Result;

        static string Result(Context _) => string.Empty;
    }

    private void AddError(string? message) {
        var error = new ValidationError(message ?? "Unknown error.", $"[{_currentToken.Line}, {_currentToken.Column}]: {_currentToken.Type}");
        _errors.Add(error);
    }

    private bool Has(TokenType type)
        => _currentToken.Type == type;

    private void Allow(TokenType type) {
        if (_currentToken.Type != type) return;
        NextToken();
    }

    private void AllowMany(TokenType type) {
        while (_currentToken.Type == type)
            NextToken();
    }

    private void Ensure(TokenType type) {
        if (_currentToken.Type != type)
            AddError($"Expected token: '{type}'.");
        NextToken();
    }

    private void Forbid(TokenType type) {
        if (_currentToken.Type != type) return;
        AddError("Token not allowed.");
        NextToken();
    }

    private string GetValueFrom(TokenType type) {
        if (_currentToken.Type != type)
            AddError($"Expected token: '{type}'.");
        var token = _currentToken;
        NextToken();
        if (token.Value is null) AddError("Token value not set.");
        return token.Value!;
    }

    private string? GetValueOrDefaultFrom(TokenType type) {
        if (_currentToken.Type != type)
            return null;
        var token = _currentToken;
        NextToken();
        return token.Value;
    }

    [MemberNotNull(nameof(_currentToken))]
    private void NextToken()
        => _currentToken = _tokens.MoveNext()
            ? _tokens.Current
            : new(TokenType.Exit, 0, 0, string.Empty);
}
