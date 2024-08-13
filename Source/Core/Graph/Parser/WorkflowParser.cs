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
        Expect(TokenType.EOL);
    }

    private void ParseIf(WorkflowBuilder builder) {
        Expect(TokenType.If);
        var predicate = ParsePredicate();
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        builder.If(tag!,
                   ctx => EvaluatePredicate(ctx, predicate),
                   b => b.IsTrue(trueBranch => {
                       Expect(TokenType.Then);
                       Expect(TokenType.EOL);
                       Expect(TokenType.Indent);
                       while (_currentToken.Type != TokenType.Dedent) {
                           ParseStatement(trueBranch);
                       }
                       Expect(TokenType.Dedent);
                   })
                         .IsFalse(falseBranch => {
                             if (_currentToken.Type == TokenType.Else) {
                                 Expect(TokenType.Else);
                                 Expect(TokenType.EOL);
                                 Expect(TokenType.Indent);
                                 while (_currentToken.Type != TokenType.Dedent) {
                                     ParseStatement(falseBranch);
                                 }
                                 Expect(TokenType.Dedent);
                             }
                         }),
                   label);
        Expect(TokenType.Dedent);
    }

    private string ParsePredicate() {
        var condition = new StringBuilder();
        while (_currentToken.Type != TokenType.EOL && _currentToken.Type != TokenType.Label) {
            condition.Append(_currentToken.Value);
            condition.Append(' ');
            NextToken();
        }
        return condition.ToString().Trim();
    }

    private void ParseCase(WorkflowBuilder builder) {
        Expect(TokenType.Case);
        var selector = GetValueFrom(TokenType.Identifier);
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        builder.Case(tag!,
                     ctx => EvaluateSelector(ctx, selector),
                     branches => {
                         while (_currentToken.Type is TokenType.Is or TokenType.Otherwise) {
                             if (_currentToken.Type is TokenType.Is) {
                                 ParseCaseOption(branches);
                                 continue;
                             }
                             ParseOtherwise(branches);
                         }
                     },
                     label);

        Expect(TokenType.Dedent);
    }

    private void ParseCaseOption(BranchingNodeBuilder branches) {
        Expect(TokenType.Is);
        var caseValue = GetValueFrom(TokenType.String);
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        branches.Is(caseValue, builder => {
            while (_currentToken.Type != TokenType.Dedent) {
                ParseStatement(builder);
            }
        });

        Expect(TokenType.Dedent);
    }

    private void ParseOtherwise(BranchingNodeBuilder branches) {
        Expect(TokenType.Otherwise);
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        branches.Otherwise(builder => {
            while (_currentToken.Type != TokenType.Dedent) {
                ParseStatement(builder);
            }
        });

        Expect(TokenType.Dedent);
    }

    private void ParseExit(WorkflowBuilder builder) {
        Expect(TokenType.Exit);
        var exitCode = int.Parse(GetValueOrDefaultFrom(TokenType.Number) ?? "0");
        var tag = GetValueOrDefaultFrom(TokenType.Tag);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        builder.End(tag, exitCode, label);
        Expect(TokenType.EOL);
    }

    private void ParseJumpTo(WorkflowBuilder builder) {
        Expect(TokenType.JumpTo);
        var target = GetValueFrom(TokenType.Identifier);
        builder.JumpTo(target);
        Expect(TokenType.EOL);
    }

    private Token Expect(TokenType type) {
        if (_currentToken.Type != type)
            throw new InvalidOperationException($"Expected {type}, but got {_currentToken.Type}");
        var token = _currentToken;
        NextToken();
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
