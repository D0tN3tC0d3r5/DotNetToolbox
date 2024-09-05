namespace AI.Sample.Agents.Handlers;

public interface IHttpConnectionHandler {
    IHttpConnection GetInternal();
    IHttpConnection Get(string modelKey);
}
