namespace Lola.Utilities.HttpConnection.Handlers;

public interface IHttpConnectionHandler {
    IAgent GetInternal();
    IAgent Get(string modelKey);
}
