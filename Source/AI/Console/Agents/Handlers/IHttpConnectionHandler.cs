namespace AI.Sample.Agents.Handlers;

public interface IHttpConnectionHandler {
    IAgent GetInternal();
    IAgent Get(string modelKey);
}
