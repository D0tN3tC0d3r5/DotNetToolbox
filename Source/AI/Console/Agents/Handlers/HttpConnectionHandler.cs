using DotNetToolbox.Http;

namespace AI.Sample.Agents.Handlers;

public class HttpConnectionHandler(IApplication application, IModelHandler modelHandler, IHttpClientProviderAccessor httpClientAccessor, ILogger<HttpConnectionHandler> logger)
    : IHttpConnectionHandler {
    private readonly IModelHandler _modelHandler = modelHandler;
    private readonly IHttpClientProviderAccessor _httpClientAccessor = httpClientAccessor;
    private readonly ILogger<HttpConnectionHandler> _logger = logger;
    private readonly IApplication _application = application;

    public IHttpConnection GetInternal() {
        var model = _modelHandler.Internal ?? throw new InvalidOperationException("No internal model found.");
        var httpClientProvider = _httpClientAccessor.Get(model.Provider!.NormalizedName);
    }

    public IHttpConnection Create() {
        var agent = _factory.Create(setUp);
        _repository.Create(setUp);
    }
}
