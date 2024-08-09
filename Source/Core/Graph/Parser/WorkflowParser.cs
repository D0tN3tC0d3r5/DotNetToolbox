namespace DotNetToolbox.Graph.Parser;

public class WorkflowParser {
    private readonly IEnumerator<Token> _tokens;
    private readonly WorkflowBuilder _builder;
    private Token _currentToken;

    public WorkflowParser(IEnumerable<Token> tokens, IServiceProvider services) {
        _tokens = tokens.GetEnumerator();
        _builder = new WorkflowBuilder(services);
        NextToken();
    }

    public WorkflowBuilder Parse() {
        while (_currentToken.Type != TokenType.End) {
            ParseStatement();
        }
        return _builder;
    }

    private void ParseStatement() {
        switch (_currentToken.Type) {
            case TokenType.Start:
                ParseStart();
                break;
            case TokenType.Action:
                ParseAction();
                break;
            case TokenType.If:
                ParseIf();
                break;
            case TokenType.When:
                ParseWhen();
                break;
            case TokenType.End:
                ParseEnd();
                break;
            default:
                throw new InvalidOperationException($"Unexpected token: {_currentToken.Type}");
        }
    }

    private void ParseStart() {
        Expect(TokenType.Start);
        Expect(TokenType.Colon);
        var label = Expect(TokenType.Identifier).Value;
        _builder.Do(label, _ => { });
        Expect(TokenType.EOL);
    }

    private void ParseAction() {
        Expect(TokenType.Action);
        Expect(TokenType.Colon);
        var label = Expect(TokenType.Identifier).Value;
        _builder.Do(label, _ => { });
        Expect(TokenType.EOL);
    }

    private void ParseIf() {
        Expect(TokenType.If);
        Expect(TokenType.Colon);
        var condition = Expect(TokenType.Identifier).Value;
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        _builder.If(ctx => EvaluateCondition(ctx, condition),
                    trueBranch => {
                        Expect(TokenType.True);
                        Expect(TokenType.Colon);
                        Expect(TokenType.EOL);
                        Expect(TokenType.Indent);
                        while (_currentToken.Type != TokenType.Dedent) {
                            ParseStatement();
                        }
                        Expect(TokenType.Dedent);
                    },
                    falseBranch => {
                        if (_currentToken.Type == TokenType.False) {
                            Expect(TokenType.False);
                            Expect(TokenType.Colon);
                            Expect(TokenType.EOL);
                            Expect(TokenType.Indent);
                            while (_currentToken.Type != TokenType.Dedent) {
                                ParseStatement();
                            }
                            Expect(TokenType.Dedent);
                        }
                    });

        Expect(TokenType.Dedent);
    }

    private void ParseWhen() {
        Expect(TokenType.When);
        Expect(TokenType.Colon);
        var selector = Expect(TokenType.Identifier).Value;
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        _builder.When(ctx => EvaluateSelector(ctx, selector),
                      branches => {
                          while (_currentToken.Type == TokenType.Case) {
                              Expect(TokenType.Case);
                              Expect(TokenType.Colon);
                              var caseValue = Expect(TokenType.Identifier).Value;
                              Expect(TokenType.EOL);
                              Expect(TokenType.Indent);

                              branches.Is(caseValue, branch => {
                                  while (_currentToken.Type != TokenType.Dedent) {
                                      ParseStatement();
                                  }
                              });

                              Expect(TokenType.Dedent);
                          }
                      });

        Expect(TokenType.Dedent);
    }

    private void ParseEnd() {
        Expect(TokenType.End);
        Expect(TokenType.Colon);
        var label = Expect(TokenType.Identifier).Value;
        _builder.End(label);
        Expect(TokenType.EOL);
    }

    private Token Expect(TokenType type) {
        if (_currentToken.Type != type) {
            throw new InvalidOperationException($"Expected {type}, but got {_currentToken.Type}");
        }
        var token = _currentToken;
        NextToken();
        return token;
    }

    private void NextToken() {
        if (_tokens.MoveNext()) {
            _currentToken = _tokens.Current;
        }
        else {
            _currentToken = new Token(TokenType.End, "", -1);
        }
    }

    private bool EvaluateCondition(Context ctx, string condition) {
        // This is a simplified implementation. In a real-world scenario,
        // you might want to use a more sophisticated expression evaluator.
        return ctx.TryGetValue(condition, out var value) && value is bool boolValue && boolValue;
    }

    private string EvaluateSelector(Context ctx, string selector) {
        // Similarly, this is a simplified implementation.
        return ctx.TryGetValue(selector, out var value) ? value?.ToString() ?? "" : "";
    }
}
