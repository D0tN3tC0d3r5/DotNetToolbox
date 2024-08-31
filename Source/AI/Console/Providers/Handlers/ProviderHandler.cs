namespace AI.Sample.Providers.Handlers;

public class ProviderHandler(IProviderRepository repository, Lazy<IModelHandler> modelHandler, ILogger<ProviderHandler> logger) : IProviderHandler {
    private readonly IProviderRepository _repository = repository;
    private readonly Lazy<IModelHandler> _modelHandler = modelHandler;
    private readonly ILogger<ProviderHandler> _logger = logger;

    public ProviderEntity[] List() => _repository.GetAll();

    public ProviderEntity? GetByKey(uint key) => _repository.FindByKey(key);
    public ProviderEntity? GetByName(string name) => _repository.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public ProviderEntity Create(Action<ProviderEntity> setUp)
        => _repository.Create(setUp);

    public void Add(ProviderEntity provider) {
        if (_repository.FindByKey(provider.Key) != null)
            throw new InvalidOperationException($"A provider with the key '{provider.Key}' already exists.");

        _repository.Add(provider);
        _logger.LogInformation("Added new provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }

    public void Update(ProviderEntity provider) {
        if (_repository.FindByKey(provider.Key) == null)
            throw new InvalidOperationException($"Provider with key '{provider.Key}' not found.");

        _repository.Update(provider);
        _logger.LogInformation("Updated provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }

    public void Remove(uint key) {
        var provider = _repository.FindByKey(key)
                     ?? throw new InvalidOperationException($"Provider with key '{key}' not found.");

        _modelHandler.Value.RemoveByProviderKey(key);
        _logger.LogInformation("Removed all models associated with provider: {ProviderKey}", key);

        _repository.Remove(key);
        _logger.LogInformation("Removed provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }
}
