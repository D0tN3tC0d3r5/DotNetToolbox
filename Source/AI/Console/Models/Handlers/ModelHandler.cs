namespace AI.Sample.Models.Handlers;

public class ModelHandler : IModelHandler {
    private const string ApplicationModelKey = "ApplicationModel";

    private readonly IApplication _application;
    private readonly IModelRepository _repository;
    private readonly Lazy<IProviderHandler> _providerHandler;
    private readonly ILogger<ModelHandler> _logger;
    private ModelEntity? _selected;

    public ModelHandler(IApplication application, IModelRepository repository, Lazy<IProviderHandler> providerHandler, ILogger<ModelHandler> logger) {
        _application = application;
        _repository = repository;
        _providerHandler = providerHandler;
        _logger = logger;
    }

    public ModelEntity? Selected {
        get => GetSelectedModel();
        private set => SetSelectedModel(IsNotNull(value));
    }

    private ModelEntity? GetSelectedModel() {
        var cachedValue = _application.Context.GetValueOrDefault<ModelEntity>(ApplicationModelKey);
        _selected = cachedValue ?? _repository.FirstOrDefault(m => m.IsSelected);
        if (cachedValue is null && _selected is not null) _application.Context[ApplicationModelKey] = _selected;
        return _selected; // Should only only return null if the storage is empty or there is no selected model in the storage.
    }

    private void SetSelectedModel(ModelEntity value) {
        if (value.Key == _selected?.Key) return;
        _selected = value;

        // Ensure record uniqueness in storage
        var oldSelectedModel = _repository.FirstOrDefault(m => m.IsSelected);
        if (oldSelectedModel is not null && oldSelectedModel.Key != _selected.Key) {
            oldSelectedModel.IsSelected = false;
            _repository.Update(oldSelectedModel);
        }
        _selected.IsSelected = true;
        _repository.Update(_selected);

        // Update cached value
        _application.Context[ApplicationModelKey] = _selected;
    }

    public ModelEntity[] List() => _repository.GetAll();

    public ModelEntity? GetByKey(string key) {
        var model = _repository.FindByKey(key);
        if (model is null || model.ProviderKey == 0) return model;
        model.Provider = _providerHandler.Value.GetByKey(model.ProviderKey);
        return model;
    }

    public ModelEntity Create(Action<ModelEntity> setUp) {
        var model = new ModelEntity();
        setUp(model);
        return model;
    }

    public void Register(ModelEntity model) {
        if (_repository.FindByKey(model.Key) is not null) {
            throw new InvalidOperationException($"A model with the key '{model.Key}' already exists.");
        }
        if (_selected is null) model.IsSelected = true;
        _repository.Add(model);
        _selected = model;
        _logger.LogInformation("Added new model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Update(ModelEntity model) {
        if (_repository.FindByKey(model.Key) == null) {
            throw new InvalidOperationException($"Settings with key '{model.Key}' not found.");
        }

        _repository.Update(model);
        _logger.LogInformation("Updated model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Remove(string key) {
        var model = _repository.FindByKey(key) ?? throw new InvalidOperationException($"Settings with key '{key}' not found.");

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

    public void Select(string key) {
        var model = _repository.FindByKey(key)
                 ?? throw new InvalidOperationException($"Settings '{key}' not found.");
        Selected = model;
        _logger.LogInformation("Settings '{ModelKey} => {ModelName}' selected : ", model.Key, model.Name);
    }
}
