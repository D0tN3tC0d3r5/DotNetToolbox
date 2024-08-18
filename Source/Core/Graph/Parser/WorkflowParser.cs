namespace DotNetToolbox.Graph.Parser;

public sealed class WorkflowParser {
    private readonly IEnumerator<Token> _tokens;
    private Token _currentToken;

    private readonly INodeSequence? _sequence;

    private WorkflowParser(IEnumerable<Token> tokens, IServiceProvider services) {
        _sequence = services.GetService<INodeSequence>() ?? NodeSequence.Shared;
        _tokens = tokens.GetEnumerator();
        NextToken();
    }

    public static Result<INode?> Parse(IEnumerable<Token> tokens, IServiceProvider services) {
        var parser = new WorkflowParser(tokens, services);
        var builder = new WorkflowBuilder(services);
        return parser.ParseStatements(builder);
    }

    private Result<INode?> ParseStatements(IWorkflowBuilder builder) {
        Result<INode?>? result = default;
        var indentColumn = _currentToken.Column;
        while (_currentToken.Type is not TokenType.EndOfFile && _currentToken.Column >= indentColumn)
            result += ParseStatement(builder);
        return result ?? Success<INode?>(null);
    }

    private Result<INode?> ParseStatement(IWorkflowBuilder builder) {
        var result = Success<INode?>(null);
        result += AllowMany(TokenType.Indent);
        return result + _currentToken.Type switch {
            TokenType.Identifier => ParseAction(builder),
            TokenType.If => ParseIf(builder),
            TokenType.Case => ParseCase(builder),
            TokenType.Exit => ParseExit(builder),
            TokenType.JumpTo => ParseJumpTo(builder),
            TokenType.EndOfLine => ParseEndOfLine(),
            TokenType.Error => ParseError(),
            _ => ParseUnknownToken(),
        };
    }

    private Result<INode?> ParseUnknownToken() {
        var result = Success<INode?>(null);
        result += CreateError("Unexpected token.");
        NextToken();
        return result;
    }

    private Result<INode?> ParseError() {
        var result = Success<INode?>(null);
        result += CreateError(_currentToken.Value);
        NextToken();
        return result;
    }

    private Result<INode?> ParseEndOfLine() {
        var result = Success<INode?>(null);
        NextToken();
        return result;
    }

    private Result<INode?> ParseAction(IWorkflowBuilder builder) {
        var result = Success<INode?>(null);
        var nameResult = GetRequiredValueFrom(TokenType.Identifier);
        result += nameResult.Errors;
        var idResult = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        result += idResult.Errors;
        var labelResult = GetValueOrDefaultFrom(TokenType.Label);
        result += labelResult.Errors;
        Ensure(TokenType.EndOfLine);
        var buildResult = BuildCommand(nameResult.Value);
        result += buildResult.Errors;

        var node = new ActionNode(idResult.Value!, buildResult.Value, _sequence);
        result += Success<INode?>(node);

        node.Label = labelResult.Value ?? nameResult.Value;
        builder.AddNode(node);
        return result;
    }

    private Result<INode?> ParseIf(IWorkflowBuilder builder) {
        var result = Success<INode?>(null);
        result += Ensure(TokenType.If);
        var idResult = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        result += idResult.Errors;
        var labelResult = GetValueOrDefaultFrom(TokenType.Label);
        result += labelResult.Errors;
        var predicateResult = ParsePredicate();
        result += predicateResult.Errors;
        result += Ensure(TokenType.EndOfLine);

        var node = new IfNode(idResult.Value!, predicateResult.Value, _sequence);
        result += Success<INode?>(node);

        node.Token = _currentToken;
        node.Label = labelResult.Value ?? node.Label;
        builder.AddNode(node);
        result += ParseThen(builder, node);
        result += ParseElse(builder, node);
        return result;
    }

    private Result<Func<Context, bool>> ParsePredicate() {
        var condition = new StringBuilder();
        while (_currentToken.Type is not TokenType.Id and not TokenType.Label and not TokenType.EndOfLine) {
            condition.Append(_currentToken.Value);
            condition.Append(' ');
            NextToken();
        }

        var expression = condition.ToString().Trim();
        return BuildPredicate(expression);
    }

    private Result ParseThen(IWorkflowBuilder builder, IIfNode node) {
        var result = ParseStatements(builder);
        node.Then = result.Value;
        return result.Errors;
    }

    private Result ParseElse(IWorkflowBuilder builder, IIfNode node) {
        var result = Success();
        if (!TokenIs(TokenType.Else))
            return result;
        result += Ensure(TokenType.Else);
        result += Ensure(TokenType.EndOfLine);
        var blockResult = ParseStatements(builder);
        node.Else = blockResult.Value;
        return result + blockResult.Errors;
    }

    private Result<INode?> ParseCase(IWorkflowBuilder builder) {
        var result = Success<INode?>(null);
        result += Ensure(TokenType.Case);
        var identifierResult = GetRequiredValueFrom(TokenType.Identifier);
        result += identifierResult.Errors;
        var idResult = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        result += idResult.Errors;
        var labelResult = GetValueOrDefaultFrom(TokenType.Label);
        result += labelResult.Errors;
        result += Ensure(TokenType.EndOfLine);

        var selectorResult = BuildSelector(identifierResult.Value);
        result += selectorResult.Errors;

        var node = new CaseNode(idResult.Value!, selectorResult.Value, _sequence);
        result += Success<INode?>(node);

        node.Label = labelResult.Value ?? node.Label;
        node.Token = _currentToken;
        builder.AddNode(node);
        result += ParseCases(builder, node);

        return result;
    }

    private Result ParseCases(IWorkflowBuilder builder, ICaseNode node) {
        var result = Success();
        while (TryParseCaseOption(builder, node, out var choiceResult))
            result += choiceResult;
        if (node.Choices.Count == 0)
            result += Forbid(TokenType.Otherwise);
        else if (TryParseOtherwise(builder, node, out var otherwiseResult))
            result += otherwiseResult;
        return result;
    }

    private bool TryParseCaseOption(IWorkflowBuilder builder, ICaseNode node, out Result result) {
        result = Success();
        if (!TokenIs(TokenType.Is)) return false;

        result += Ensure(TokenType.Is);
        var choiceKeyResult = GetRequiredValueFrom(TokenType.String);
        result += choiceKeyResult.Errors;
        result += Ensure(TokenType.EndOfLine);
        var blockResult = ParseStatements(builder);
        result += blockResult.Errors;
        node.Choices.Add(choiceKeyResult.Value, blockResult.Value);
        return true;
    }

    private bool TryParseOtherwise(IWorkflowBuilder builder, ICaseNode node, out Result result) {
        result = Success();
        if (!TokenIs(TokenType.Otherwise)) return false;

        result += Ensure(TokenType.Otherwise);
        result += Ensure(TokenType.EndOfLine);
        var blockResult = ParseStatements(builder);
        result += blockResult.Errors;
        node.Choices.Add(string.Empty, blockResult.Value);
        result += Forbid(TokenType.Otherwise);
        return true;
    }

    private Result<INode?> ParseExit(IWorkflowBuilder builder) {
        var result = Success<INode?>(null);
        result += Ensure(TokenType.Exit);
        var numberResult = GetValueOrDefaultFrom(TokenType.Number, "0"); // if number not found default to 0
        if (int.TryParse(numberResult.Value, out var exitCode))
            result += CreateError("Exit code must be a number.");
        var idResult = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        result += idResult.Errors;

        var node = new ExitNode(idResult.Value!, exitCode, _sequence);
        result += Success<INode?>(node);

        var labelResult = GetValueOrDefaultFrom(TokenType.Label);
        result += labelResult.Errors;
        node.Label = labelResult.Value ?? node.Label;
        node.Token = _currentToken;
        builder.AddNode(node);

        result += Ensure(TokenType.EndOfLine);
        return result;
    }

    private Result<INode?> ParseJumpTo(IWorkflowBuilder builder) {
        var result = Success<INode?>(null);
        result += Ensure(TokenType.JumpTo);
        var targetResult = GetRequiredValueFrom(TokenType.Identifier);
        result += targetResult.Errors;
        var idResult = GetValueOrDefaultFrom(TokenType.Id, string.Empty);
        result += idResult.Errors;

        var node = new JumpNode(idResult.Value!, targetResult.Value, _sequence);
        result += Success<INode?>(node);

        var labelResult = GetValueOrDefaultFrom(TokenType.Label);
        result += labelResult.Errors;
        node.Label = labelResult.Value ?? node.Label;
        node.Token = _currentToken;
        builder.AddNode(node);

        result += Ensure(TokenType.EndOfLine);
        return result;
    }

    private Result<Action<Context>> BuildCommand(string action) {
        // fetch or build the action expression;
        return Success(Command);

        static void Command(Context _) { }
    }
    private Result<Func<Context, bool>> BuildPredicate(string condition) {
        // fetch or build the predicateExpression expression;
        return Success(Predicate);

        static bool Predicate(Context _) => true;
    }

    private Result<Func<Context, string>> BuildSelector(string selector) {
        // fetch or build the selector expression;
        return Success(Selector);

        static string Selector(Context _) => string.Empty;
    }

    private Result CreateError(string? message)
        => new ValidationError(message ?? "Unknown error.", $"[{_currentToken.Line}, {_currentToken.Column}]: {_currentToken.Type}");

    private bool TokenIs(TokenType type)
        => _currentToken.Type == type;

    private Result AllowMany(TokenType type) {
        var result = Success();
        while (_currentToken.Type == type)
            NextToken();
        return result;
    }

    private Result Ensure(TokenType type) {
        var result = Success();
        if (_currentToken.Type != type)
            result += CreateError($"Expected token: '{type}'.");
        NextToken();
        return result;
    }

    private Result Forbid(TokenType type) {
        if (_currentToken.Type != type) Success();
        var result = CreateError("Token not allowed.");
        NextToken();
        return result;
    }

    private Result<string> GetRequiredValueFrom(TokenType type) {
        var result = Success(_currentToken.Value!);
        if (_currentToken.Type != type)
            result += CreateError($"Expected token: '{type}'.");
        if (_currentToken.Value is null)
            result += CreateError("Token value not set.");
        NextToken();
        return result;
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    private Result<string?> GetValueOrDefaultFrom(TokenType type, string? defaultValue = null) {
        var result = Success(_currentToken.Value);
        if (_currentToken.Type != type)
            result += CreateError($"Expected token: '{type}'.");
        NextToken();
        return result;
    }

    [MemberNotNull(nameof(_currentToken))]
    private void NextToken()
        => _currentToken = _tokens.MoveNext()
           ? _tokens.Current
           : new(TokenType.Exit, 0, 0, string.Empty);
}
