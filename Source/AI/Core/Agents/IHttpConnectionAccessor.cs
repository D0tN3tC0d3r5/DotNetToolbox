namespace DotNetToolbox.AI.Agents;

public interface IHttpConnectionAccessor {
    IHttpConnection GetFor(string provider);
}
