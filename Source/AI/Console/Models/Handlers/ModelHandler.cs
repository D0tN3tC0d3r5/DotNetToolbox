namespace AI.Sample.Models.Handlers;

public class ModelHandler(IModelRepository repository, Lazy<IProviderHandler> providerHandler, ILogger<ModelHandler> logger) : IModelHandler {
    private readonly IModelRepository _repository = repository;
    private readonly Lazy<IProviderHandler> _providerHandler = providerHandler;
    private readonly ILogger<ModelHandler> _logger = logger;

    public ModelEntity[] List() => _repository.GetAll();

    public ModelEntity? GetByKey(string key) => _repository.FindByKey(key);

    public ModelEntity Create(Action<ModelEntity> setUp) {
        var model = new ModelEntity();
        setUp(model);
        return model;
    }

    public void Register(ModelEntity model) {
        if (_repository.FindByKey(model.Key) != null) {
            throw new InvalidOperationException($"A model with the key '{model.Key}' already exists.");
        }

        _repository.Add(model);
        _logger.LogInformation("Added new model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Update(ModelEntity model) {
        if (_repository.FindByKey(model.Key) == null) {
            throw new InvalidOperationException($"Model with key '{model.Key}' not found.");
        }

        _repository.Update(model);
        _logger.LogInformation("Updated model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Remove(string key) {
        var model = _repository.FindByKey(key) ?? throw new InvalidOperationException($"Model with key '{key}' not found.");

        _repository.Remove(key);
        _logger.LogInformation("Removed model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public ModelEntity[] ListByProvider(string provider) {
        if (uint.TryParse(provider, out var key))
            return _repository.GetByProviderKey(key);
        var entity = _providerHandler.Value.GetByName(provider);
        return entity is null
            ? []
            : _repository.GetByProviderKey(entity.Key);
    }

    public void RemoveByProviderKey(uint providerKey) {
        foreach (var model in _repository.GetByProviderKey(providerKey)) {
            _repository.Remove(model.Key);
            _logger.LogInformation("Removed model associated with provider {ProviderKey}: {ModelKey} => {ModelName}",
                providerKey, model.Key, model.Name);
        }
    }
}
