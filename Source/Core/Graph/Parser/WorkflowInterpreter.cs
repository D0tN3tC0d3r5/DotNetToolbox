namespace DotNetToolbox.Graph.Parser;

public class WorkflowInterpreter(IServiceProvider services) {
    public WorkflowBuilder InterpretScript(string script) {
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize();
        var parser = new WorkflowParser(tokens, services);
        return parser.Parse();
    }
}
