namespace DotNetToolbox.Graph.Parser;

public sealed class WorkflowParser {
    private readonly IEnumerator<Token> _tokens;
    private readonly INodeSequence? _sequence;

    private Token _currentToken;

    private WorkflowParser(IEnumerable<Token> tokens, IServiceProvider services) {
        _sequence = services.GetService<INodeSequence>() ?? NodeSequence.Singleton;
        _tokens = tokens.GetEnumerator();
        NextToken();
    }

    internal ValidationErrors Errors { get; } = [];

    public static Result<INode?> Parse(IEnumerable<Token> tokens, IServiceProvider services) {
        var parser = new WorkflowParser(tokens, services);
        return new(parser.ParseBlock(), parser.Errors);
    }

    private INode? ParseBlock() {
        var nodes = new List<INode>();
        var indentColumn = _currentToken.Column;
        while (_currentToken.Type is not TokenType.EndOfFile && _currentToken.Column >= indentColumn)
            nodes.Add(ParseStatement());
        return nodes.FirstOrDefault();
    }

    private INode ParseStatement() {
        AllowMany(TokenType.Indent);
        return _currentToken.Type switch {
            TokenType.Identifier => ParseAction(),
            TokenType.If => ParseIf(),
            TokenType.Case => ParseCase(),
            TokenType.Exit => ParseExit(),
            TokenType.JumpTo => ParseJumpTo(),
            TokenType.EndOfLine => ParseEndOfLine(),
            TokenType.Error => ParseError(),
            _ => ParseUnknownToken(),
        };
    }

    private INode ParseUnknownToken() {
        AddError("Unexpected token.");
        NextToken();
        return null!;
    }

    private INode ParseError() {
        AddError(_currentToken.Value);
        NextToken();
        return null!;
    }

    private INode ParseEndOfLine() {
        NextToken();
        return null!;
    }

    private INode ParseAction() {
        var name = GetRequiredValueFrom(TokenType.Identifier);
        var id = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Ensure(TokenType.EndOfLine);
        var command = BuildCommand(name);
        return new ActionNode(id, command, _sequence) {
            Token = _currentToken,
            Label = label ?? name,
        };
    }

    private INode ParseIf() {
        Ensure(TokenType.If);
        var id = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        var predicate = ParsePredicate();
        Ensure(TokenType.EndOfLine);

        var node = new IfNode(id, predicate, _sequence) {
            Token = _currentToken,
            Then = ParseBlock(),
            Else = ParseElse(),
        };
        node.Label = label ?? node.Label;
        return node;
    }

    private Func<Context, bool> ParsePredicate() {
        var condition = new StringBuilder();
        while (_currentToken.Type is not TokenType.Id and not TokenType.Label and not TokenType.EndOfLine) {
            condition.Append(_currentToken.Value);
            condition.Append(' ');
            NextToken();
        }

        var expression = condition.ToString().Trim();
        return BuildPredicate(expression);
    }

    private INode? ParseElse() {
        if (!TokenIs(TokenType.Else))
            return null;
        Ensure(TokenType.Else);
        Ensure(TokenType.EndOfLine);
        return ParseBlock();
    }

    private INode ParseCase() {
        Ensure(TokenType.Case);
        var identifier = GetRequiredValueFrom(TokenType.Identifier);
        var id = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Ensure(TokenType.EndOfLine);

        var selector = BuildSelector(identifier);
        var node = new CaseNode(id, selector, _sequence) { Token = _currentToken, };
        node.Label = label ?? node.Label;
        foreach ((var key, var choice) in ParseChoices())
            node.Choices.Add(key, choice);
        return node;
    }

    private IEnumerable<(string, INode?)> ParseChoices() {
        while (TryParseCaseOption(out var choice))
            yield return choice;
        if (TryParseOtherwise(out var otherwise))
            yield return (string.Empty, otherwise);
    }

    private bool TryParseCaseOption(out (string, INode?) result) {
        result = default;
        if (!TokenIs(TokenType.Is)) return false;

        Ensure(TokenType.Is);
        var key = GetRequiredValueFrom(TokenType.String);
        Ensure(TokenType.EndOfLine);
        result = (key, ParseBlock());
        return true;
    }

    private bool TryParseOtherwise(out INode? result) {
        result = default;
        if (!TokenIs(TokenType.Otherwise)) return false;

        Ensure(TokenType.Otherwise);
        Ensure(TokenType.EndOfLine);
        result = ParseBlock();
        Forbid(TokenType.Otherwise);
        return true;
    }

    private INode ParseExit() {
        Ensure(TokenType.Exit);
        var number = GetValueOrDefaultFrom(TokenType.Number, "0"); // if number not found default to 0
        if (!int.TryParse(number, out var exitCode))
            AddError("Exit code must be a number.");
        var id = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Ensure(TokenType.EndOfLine);

        var node = new ExitNode(id, exitCode, _sequence) { Token = _currentToken };
        node.Label = label ?? node.Label;
        return node;
    }

    private INode ParseJumpTo() {
        Ensure(TokenType.JumpTo);
        var target = GetRequiredValueFrom(TokenType.Identifier);
        var id = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        var label = GetValueOrDefaultFrom(TokenType.Label);
        Ensure(TokenType.EndOfLine);

        var node = new JumpNode(id, target, _sequence) { Token = _currentToken };
        node.Label = label ?? node.Label;
        return node;
    }

    private Action<Context> BuildCommand(string action) {
        // fetch or build the action expression;
        return Command;

        static void Command(Context _) { }
    }
    private Func<Context, bool> BuildPredicate(string condition) {
        // fetch or build the predicateExpression expression;
        return Predicate;

        static bool Predicate(Context _) => true;
    }

    private Func<Context, string> BuildSelector(string selector) {
        // fetch or build the selector expression;
        return Selector;

        static string Selector(Context _) => string.Empty;
    }

    private void AddError(string? message)
        => Errors.Add(new ValidationError(message ?? "Unknown error.", $"[{_currentToken.Line}, {_currentToken.Column}]: {_currentToken.Type}"));

    private bool TokenIs(TokenType type)
        => _currentToken.Type == type;

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
        if (_currentToken.Type == type)
            AddError("Token not allowed.");
        NextToken();
    }

    private string GetRequiredValueFrom(TokenType type) {
        if (_currentToken.Type != type)
            AddError($"Expected token: '{type}'.");
        if (_currentToken.Value is null)
            AddError("Token value not set.");
        NextToken();
        return _currentToken.Value!;
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    private string? GetValueOrDefaultFrom(TokenType type, string? defaultValue = null) {
        if (_currentToken.Type != type)
            AddError($"Expected token: '{type}'.");
        NextToken();
        return _currentToken.Value ?? defaultValue;
    }

    [MemberNotNull(nameof(_currentToken))]
    private void NextToken()
        => _currentToken = _tokens.MoveNext()
           ? _tokens.Current
           : new(TokenType.Exit, 0, 0, string.Empty);
}
