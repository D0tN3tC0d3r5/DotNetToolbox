namespace DotNetToolbox.Graph.Parser;

public class WorkflowInterpreter {
    private readonly IServiceProvider _services;

    public WorkflowInterpreter(IServiceProvider services) {
        _services = services;
    }

    public WorkflowBuilder InterpretScript(string script) {
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize();
        var parser = new WorkflowParser(tokens, _services);
        return parser.Parse();
    }
}