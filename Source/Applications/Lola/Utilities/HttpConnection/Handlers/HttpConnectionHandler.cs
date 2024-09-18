namespace Lola.Utilities.HttpConnection.Handlers;

public class HttpConnectionHandler(IModelHandler modelHandler, IAgentAccessor httpConnectionAccessor)
    : IHttpConnectionHandler {
    public IAgent GetInternal() {
        var model = modelHandler.Selected ?? throw new InvalidOperationException("No internal model found.");
        return httpConnectionAccessor.GetFor(model.Provider!.Name);
    }

    public IAgent Get(string modelKey) {
        var model = modelHandler.GetByKey(modelKey) ?? throw new InvalidOperationException("Model not found.");
        return httpConnectionAccessor.GetFor(model.Provider!.Name);
    }
}
