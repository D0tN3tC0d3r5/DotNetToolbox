namespace DotNetToolbox.Graph.Parser;

public sealed class WorkflowParser {
    private readonly IEnumerator<Token> _tokens;
    private readonly WorkflowBuilder _builder;
    private Token _currentToken;

    private WorkflowParser(IEnumerable<Token> tokens, IServiceProvider services) {
        _tokens = tokens.GetEnumerator();
        _builder = new WorkflowBuilder(services);
        NextToken();
    }

    public static INode? Parse(IEnumerable<Token> tokens, IServiceProvider services) {
        var parser = new WorkflowParser(tokens, services);
        return parser.Process();
    }

    public INode? Process() {
        while (_currentToken.Type is not TokenType.EOF) {
            ParseStatement(_builder);
        }
        return _builder.Start;
    }

    private void ParseStatement(WorkflowBuilder builder) {
        switch (_currentToken.Type) {
            case TokenType.Identifier:
                ParseAction(builder);
                break;
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
            case TokenType.EOL: // Empty line
                NextToken();
                break;
            default:
                throw new InvalidOperationException($"Unexpected token: {_currentToken.Type}");
        }
    }

    private void ParseAction(WorkflowBuilder builder) {
        var name = GetValueFrom(TokenType.Identifier);
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label) ?? name;
        builder.Do(tag!,
                   ctx => EvaluateAction(ctx, name),
                   label);
        GetRequired(TokenType.EOL);
    }

    private void ParseIf(WorkflowBuilder builder) {
        Ensure(TokenType.If);
        var predicate = ParsePredicate();
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Allow(TokenType.Then);
        Ensure(TokenType.EOL);
        Ensure(TokenType.Indent);

        builder.If(tag!,
                   ctx => EvaluatePredicate(ctx, predicate),
                   b => b.IsTrue(ParseThen)
                         .IsFalse(ParseElse),
                   label);
    }

    private string ParsePredicate() {
        var condition = new StringBuilder();
        while (_currentToken.Type is not TokenType.EOL and not TokenType.Label) {
            condition.Append(_currentToken.Value);
            condition.Append(' ');
            NextToken();
        }
        return condition.ToString().Trim();
    }

    private void ParseThen(WorkflowBuilder builder) {
        var indentColumn = _currentToken.Column;
        Allow(TokenType.Else);
        Ensure(TokenType.EOL);
        Ensure(TokenType.Indent);
        while (_currentToken.Column > indentColumn) {
            ParseStatement(builder);
        }
    }

    private void ParseElse(WorkflowBuilder builder) {
        var indentColumn = _currentToken.Column;
        if (!Has(TokenType.Else))
            return;

        Ensure(TokenType.EOL);
        Ensure(TokenType.Indent);
        while (_currentToken.Column > indentColumn) {
            ParseStatement(builder);
        }
    }

    private void ParseCase(WorkflowBuilder builder) {
        GetRequired(TokenType.Case);
        var selector = GetValueFrom(TokenType.Identifier);
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        GetRequired(TokenType.EOL);
        GetRequired(TokenType.Indent);

        builder.Case(tag!,
                     ctx => EvaluateSelector(ctx, selector),
                     ParseCaseOptions,
                     label);
    }

    private void ParseCaseOptions(BranchingNodeBuilder branches) {
        var count = 0;
        while (TryParseCaseOption(branches)) count++;
        if (count == 0) Forbid(TokenType.Otherwise);
        ParseOtherwise(branches);
    }

    private bool TryParseCaseOption(BranchingNodeBuilder branches) {
        var indentColumn = _currentToken.Column;
        if (!Has(TokenType.Is)) return false;
        var caseValue = GetValueFrom(TokenType.String);
        GetRequired(TokenType.EOL);
        GetRequired(TokenType.Indent);
        branches.Is(caseValue, builder => {
            while (_currentToken.Column > indentColumn) {
                ParseStatement(builder);
            }
        });
        return true;
    }

    private void ParseOtherwise(BranchingNodeBuilder branches) {
        var indentColumn = _currentToken.Column;
        if (!Has(TokenType.Otherwise)) return;
        GetRequired(TokenType.EOL);
        GetRequired(TokenType.Indent);

        branches.Otherwise(builder => {
            while (_currentToken.Type != TokenType.Indent || _currentToken.Column > indentColumn) {
                ParseStatement(builder);
            }
        });
        Forbid(TokenType.Otherwise);
    }

    private void ParseExit(WorkflowBuilder builder) {
        GetRequired(TokenType.Exit);
        var exitCode = int.Parse(GetValueOrDefaultFrom(TokenType.Number) ?? "0");
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        builder.End(tag, exitCode, label);
        GetRequired(TokenType.EOL);
    }

    private void ParseJumpTo(WorkflowBuilder builder) {
        GetRequired(TokenType.JumpTo);
        var target = GetValueFrom(TokenType.Identifier);
        builder.JumpTo(target);
        GetRequired(TokenType.EOL);
    }

    private bool Has(TokenType type) {
        if (_currentToken.Type != type) return false;
        NextToken();
        return true;
    }

    private void Allow(TokenType type) {
        if (_currentToken.Type != type) return;
        NextToken();
    }

    private void Ensure(TokenType type) {
        if (_currentToken.Type != type)
            throw new InvalidOperationException($"Expected {type}, but got {_currentToken.Type}");
        NextToken();
    }

    private void Forbid(TokenType type) {
        if (_currentToken.Type == type)
            throw new InvalidOperationException($"{_currentToken.Type} is not allowed.");
    }

    private Token GetRequired(TokenType type) {
        var token = _currentToken;
        Ensure(type);
        return token;
    }

    private string GetValueFrom(TokenType type) {
        if (_currentToken.Type != type)
            throw new InvalidOperationException($"Expected token of type '{type}', but found '{_currentToken.Type}'");
        var token = _currentToken;
        NextToken();
        return token.Value ?? throw new InvalidOperationException($"The value for '{type}' token is not set.");
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
            : new Token(TokenType.Exit, 0, 0, string.Empty);

    // This is a simplified implementation.
    // In a real-world scenario, you might want to use a more sophisticated expression evaluator.
    private static Action<Context> EvaluateAction(Context ctx, string action)
        => ctx.TryGetValue(action, out var value) && value is Action<Context> execute
            ? execute
            : _ => { };

    // This is a simplified implementation.
    // In a real-world scenario, you might want to use a more sophisticated expression evaluator.
    private static bool EvaluatePredicate(Context ctx, string condition)
        => ctx.TryGetValue(condition, out var value)
            && value is bool boolValue
            && boolValue;

    // This is a simplified implementation.
    // In a real-world scenario, you might want to use a more sophisticated expression evaluator.
    private static string EvaluateSelector(Context ctx, string selector)
        => ctx.TryGetValue(selector, out var value)
            ? value?.ToString() ?? string.Empty
            : string.Empty;
}
