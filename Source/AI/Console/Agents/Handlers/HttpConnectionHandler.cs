using DotNetToolbox.Http;

namespace AI.Sample.Agents.Handlers;

public class HttpConnectionHandler(IModelHandler modelHandler, IHttpConnectionAccessor httpConnectionAccessor)
    : IHttpConnectionHandler {
    private readonly IModelHandler _modelHandler = modelHandler;
    private readonly IHttpConnectionAccessor _httpConnectionAccessor = httpConnectionAccessor;

    public IHttpConnection GetInternal() {
        var model = _modelHandler.Internal ?? throw new InvalidOperationException("No internal model found.");
        return _httpConnectionAccessor.GetFor(model.Provider!.NormalizedName);
    }

    public IHttpConnection Get(string modelKey) {
        var model = _modelHandler.GetByKey(modelKey) ?? throw new InvalidOperationException("Model not found.");
        return _httpConnectionAccessor.GetFor(model.Provider!.NormalizedName);
    }
}
