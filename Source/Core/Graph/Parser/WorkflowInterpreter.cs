﻿namespace DotNetToolbox.Graph.Parser;

public class WorkflowInterpreter(IServiceProvider services) {
    public INode? InterpretScript(string script) {
        var tokens = WorkflowLexer.Tokenize(script);
        return WorkflowParser.Parse(tokens, services);
    }
}