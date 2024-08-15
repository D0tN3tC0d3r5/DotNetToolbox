namespace DotNetToolbox.Graph.Parser;

public class WorkflowInterpreter(IServiceProvider services) {
    public Result<INode?> InterpretScript(string script) {
        var tokens = WorkflowLexer.Tokenize(script);
        return WorkflowParser.Parse(tokens, services);
    }
}
