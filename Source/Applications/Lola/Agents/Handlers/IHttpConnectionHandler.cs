namespace Lola.Agents.Handlers;

public interface IHttpConnectionHandler {
    IAgent GetInternal();
    IAgent Get(string modelKey);
}
