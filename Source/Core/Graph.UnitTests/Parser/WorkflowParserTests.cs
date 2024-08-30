namespace DotNetToolbox.Graph.Parser;

public partial class WorkflowParserTests {
    private readonly IServiceProvider _services;

    protected WorkflowParserTests() {
        var services = new ServiceCollection();
        services.AddScoped<INodeSequencer, NodeSequencer>();
        services.AddSingleton<INodeFactory, NodeFactory>();
        services.AddScoped<IRetryPolicy, RetryPolicy>();
        _services = services.BuildServiceProvider();
    }
}
