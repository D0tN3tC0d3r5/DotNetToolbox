namespace DotNetToolbox.Graph.Parser;

public sealed class WorkflowParser {
    private readonly IEnumerator<Token> _tokens;
    private readonly INodeSequence? _sequence;

    private int _indentLevel = -1;

    private WorkflowParser(IEnumerable<Token> tokens, IServiceProvider services) {
        _sequence = services.GetService<INodeSequence>() ?? NodeSequence.Transient;
        _tokens = tokens.GetEnumerator();
        _tokens.MoveNext();
    }

    internal ValidationErrors Errors { get; } = [];

    public static Result<INode?> Parse(IEnumerable<Token> tokens, IServiceProvider services) {
        var parser = new WorkflowParser(tokens, services);
        return new(parser.ParseBlock(), parser.Errors);
    }

    private INode? ParseBlock() {
        EnsureIndent();
        var nodes = new List<INode>();
        var lastNode = default(INode);
        var finishBlock = IsEndOfFile();
        while (!finishBlock) {
            var newNode = ParseStatement();
            lastNode?.ConnectTo(newNode);
            lastNode = newNode;
            nodes.Add(newNode);
            finishBlock = IsEndOfFile() || IsDedented();
        }
        return nodes.FirstOrDefault();
    }

    private bool IsEndOfFile()
        => _tokens.Current.Type is TokenType.EndOfFile;

    private void EnsureIndent() {
        var count = CountToken(TokenType.Indent);
        var isIndented = _indentLevel < count;
        if (_indentLevel > count) AddError($"Wrong indentation level. Expected '{_indentLevel}' but found {count}.");
        if (isIndented) _indentLevel = count;
    }

    private bool IsDedented() {
        var count = CountToken(TokenType.Indent);
        var isDedented = _indentLevel > count;
        if (_indentLevel < count) AddError($"Wrong indentation level. Expected '{_indentLevel}' but found {count}.");
        if (isDedented) _indentLevel = count;
        return isDedented;
    }

    private INode ParseStatement()
        => _tokens.Current.Type switch {
            TokenType.Identifier => ParseAction(),
            TokenType.If => ParseIf(),
            TokenType.Case => ParseCase(),
            TokenType.Exit => ParseExit(),
            TokenType.JumpTo => ParseJumpTo(),
            TokenType.EndOfLine => ParseEndOfLine(),
            TokenType.Error => ParseError(),
            _ => ParseUnknownToken(),
        };

    private INode ParseUnknownToken() {
        AddError("Unexpected token.");
        _tokens.MoveNext();
        return null!;
    }

    private INode ParseError() {
        AddError(_tokens.Current.Value);
        _tokens.MoveNext();
        return null!;
    }

    private INode ParseEndOfLine() {
        _tokens.MoveNext();
        return null!;
    }

    private INode ParseAction() {
        var token = _tokens.Current;
        var name = GetValue(TokenType.Identifier);
        var id = GetValueOrDefault(TokenType.Id, string.Empty);
        var label = GetValueOrDefault(TokenType.Label);
        Ensure(TokenType.EndOfLine);
        var command = BuildCommand(name);
        return new ActionNode(id, command, _sequence) {
            Token = token,
            Label = label ?? name,
        };
    }

    private INode ParseIf() {
        var token = _tokens.Current;
        Ensure(TokenType.If);
        var id = GetValueOrDefault(TokenType.Id, string.Empty);
        var label = GetValueOrDefault(TokenType.Label);
        var predicate = ParsePredicate();
        Ensure(TokenType.EndOfLine);
        var node = new IfNode(id, predicate, _sequence) {
            Token = token,
            Then = ParseBlock(),
            Else = ParseElse(),
        };
        node.Label = label ?? node.Label;
        return node;
    }

    private Func<Context, bool> ParsePredicate() {
        var condition = new StringBuilder();
        while (_tokens.Current.Type is not TokenType.Id and not TokenType.Label and not TokenType.EndOfLine) {
            condition.Append(_tokens.Current.Value);
            condition.Append(' ');
            _tokens.MoveNext();
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
        var token = _tokens.Current;
        Ensure(TokenType.Case);
        var identifier = GetValue(TokenType.Identifier);
        var id = GetValueOrDefault(TokenType.Id, string.Empty);
        var label = GetValueOrDefault(TokenType.Label);
        Ensure(TokenType.EndOfLine);

        var selector = BuildSelector(identifier);
        var node = new CaseNode(id, selector, _sequence) { Token = token };
        node.Label = label ?? node.Label;
        foreach ((var key, var choice) in ParseChoices())
            node.Choices.Add(key, choice);
        return node;
    }

    private IEnumerable<(string, INode?)> ParseChoices() {
        _indentLevel++;
        while (TryParseCaseOption(out var choice))
            yield return choice;
        if (TryParseOtherwise(out var otherwise))
            yield return (string.Empty, otherwise);
        _indentLevel--;
    }

    private bool TryParseCaseOption(out (string, INode?) result) {
        result = default;
        if (!TokenIs(TokenType.Is)) return false;

        Ensure(TokenType.Is);
        var key = GetValue(TokenType.String);
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
        var token = _tokens.Current;
        Ensure(TokenType.Exit);
        var number = GetValueOrDefault(TokenType.Number, "0"); // if number not found default to 0
        if (!int.TryParse(number, out var exitCode))
            AddError("Exit code must be a number.");
        var id = GetValueOrDefault(TokenType.Id, string.Empty);
        var label = GetValueOrDefault(TokenType.Label);
        Ensure(TokenType.EndOfLine);

        var node = new ExitNode(id, exitCode, _sequence) { Token = token };
        node.Label = label ?? node.Label;
        return node;
    }

    private INode ParseJumpTo() {
        var token = _tokens.Current;
        Ensure(TokenType.JumpTo);
        var target = GetValue(TokenType.Identifier);
        var id = GetValueOrDefault(TokenType.Id, string.Empty);
        var label = GetValueOrDefault(TokenType.Label);
        Ensure(TokenType.EndOfLine);

        var node = new JumpNode(id, target, _sequence) { Token = token };
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
        => Errors.Add(new ValidationError(message ?? "Unknown error.", $"[{_tokens.Current.Line}, {_tokens.Current.Column}]: {_tokens.Current.Type}"));

    private int CountToken(TokenType type) {
        var count = 0;
        while (TokenIs(type)) {
            count++;
            _tokens.MoveNext();
        }

        return count;
    }
    private bool TokenIs(TokenType type)
        => _tokens.Current.Type == type;

    private void Ensure(TokenType type) {
        if (!TokenIs(type)) {
            AddError($"Expected token: '{type}'.");
            return;
        }
        _tokens.MoveNext();
    }

    private void Forbid(TokenType type) {
        if (TokenIs(type))
            AddError("Token not allowed.");
        _tokens.MoveNext();
    }

    private string GetValue(TokenType type) {
        if (!TokenIs(type)) {
            AddError($"Expected token: '{type}'.");
            return "[Invalid Token]";
        }
        var value = _tokens.Current.Value;
        _tokens.MoveNext();
        return value!;
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    private string? GetValueOrDefault(TokenType type, string? defaultValue = null) {
        if (!TokenIs(type))
            return defaultValue;
        var value = _tokens.Current.Value ?? defaultValue;
        _tokens.MoveNext();
        return value;
    }
}
