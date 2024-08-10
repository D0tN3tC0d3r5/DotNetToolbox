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
        while (_currentToken.Type != TokenType.Exit) {
            ParseStatement();
        }
        return _builder;
    }

    private void ParseStatement() {
        switch (_currentToken.Type) {
            case TokenType.Identifier:
                ParseAction();
                break;
            case TokenType.If:
                ParseIf();
                break;
            case TokenType.When:
                ParseWhen();
                break;
            case TokenType.Exit:
                ParseExit();
                break;
            default:
                throw new InvalidOperationException($"Unexpected token: {_currentToken.Type}");
        }
    }

    private void ParseAction() {
        Expect(TokenType.Identifier);
        var name = Expect(TokenType.Identifier).Value.ToString()!;
        _builder.Do(name, _ => { });
        Expect(TokenType.EOL);
    }

    private void ParseIf() {
        Expect(TokenType.If);
        var condition = Expect(TokenType.Identifier).Value.ToString()!;
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        _builder.If(ctx => EvaluateCondition(ctx, condition),
                    trueBranch => {
                        Expect(TokenType.Then);
                        Expect(TokenType.EOL);
                        Expect(TokenType.Indent);
                        while (_currentToken.Type != TokenType.EOS) {
                            ParseStatement();
                        }
                        Expect(TokenType.EOS);
                    },
                    falseBranch => {
                        if (_currentToken.Type == TokenType.Else) {
                            Expect(TokenType.Else);
                            Expect(TokenType.EOL);
                            Expect(TokenType.Indent);
                            while (_currentToken.Type != TokenType.EOS) {
                                ParseStatement();
                            }
                            Expect(TokenType.EOS);
                        }
                    });

        Expect(TokenType.EOS);
    }

    private void ParseWhen() {
        Expect(TokenType.When);
        var selector = Expect(TokenType.Identifier).Value.ToString()!;
        Expect(TokenType.EOL);
        Expect(TokenType.Indent);

        _builder.When(ctx => EvaluateSelector(ctx, selector),
                      branches => {
                          while (_currentToken.Type == TokenType.Is) {
                              Expect(TokenType.Is);
                              var caseValue = Expect(TokenType.Identifier).Value.ToString()!;
                              Expect(TokenType.EOL);
                              Expect(TokenType.Indent);

                              branches.Is(caseValue, branch => {
                                  while (_currentToken.Type != TokenType.EOS) {
                                      ParseStatement();
                                  }
                              });

                              Expect(TokenType.EOS);
                          }
                      });

        Expect(TokenType.EOS);
    }

    private void ParseExit() {
        Expect(TokenType.Exit);
        var label = Expect(TokenType.Identifier).Value.ToString()!;
        _builder.End(label);
        Expect(TokenType.EOL);
    }

    private Token Expect(TokenType type) {
        if (_currentToken.Type != type)
            throw new InvalidOperationException($"Expected {type}, but got {_currentToken.Type}");
        var token = _currentToken;
        NextToken();
        return token;
    }

    private void NextToken()
        => _currentToken = _tokens.MoveNext()
            ? _tokens.Current
            : new Token(TokenType.Exit, "", 0, 0);

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
