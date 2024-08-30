namespace DotNetToolbox.Graph.Nodes;

public partial class NodeTests {
    private static IServiceProvider CreateServiceProvider(IRetryPolicy? policy = null) {
        var services = new ServiceCollection();
        services.AddSingleton<INodeFactory, NodeFactory>();
        services.AddScoped<INodeSequencer, NodeSequencer>();
        services.AddScoped(_ => policy ?? RetryPolicy.Default);
        return services.BuildServiceProvider();
    }

    private static INodeFactory CreateFactory(IRetryPolicy? policy = null) {
        var provider = CreateServiceProvider(policy);
        return new NodeFactory(provider);
    }
}
