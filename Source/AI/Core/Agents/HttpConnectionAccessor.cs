namespace DotNetToolbox.AI.Agents;

public class HttpConnectionAccessor(IServiceProvider services)
    : IHttpConnectionAccessor {
    public IHttpConnection GetFor(string provider)
        => services.GetRequiredKeyedService<IHttpConnection>(IsNotNull(provider));
}
