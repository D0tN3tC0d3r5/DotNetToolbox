namespace AI.Sample.Models.Handlers;

public class ModelHandler(IApplication application, IModelRepository repository, ILogger<ModelHandler> logger)
    : IModelHandler {
    private const string ApplicationModelKey = "ApplicationModel";

    private readonly IApplication _application = application;
    private readonly IModelRepository _repository = repository;
    private readonly ILogger<ModelHandler> _logger = logger;
    private ModelEntity? _selected;

    public ModelEntity? Internal {
        get => GetSelected();
        private set => SetSelected(IsNotNull(value));
    }

    private ModelEntity? GetSelected() {
        var cachedValue = _application.Context.GetValueOrDefaultAs<ModelEntity>(ApplicationModelKey);
        _selected = cachedValue ?? _repository.FirstOrDefault(m => m.Selected);
        if (cachedValue is null && _selected is not null) _application.Context[ApplicationModelKey] = _selected;
        return _selected; // Should only return null if the storage is empty or there is no selected model in the storage.
    }

    private void SetSelected(ModelEntity value) {
        if (value.Key == _selected?.Key) return;
        _selected = value;

        // Ensure record uniqueness in storage
        var oldSelectedModel = _repository.FirstOrDefault(m => m.Selected);
        if (oldSelectedModel is not null && oldSelectedModel.Key != _selected.Key) {
            oldSelectedModel.Selected = false;
            _repository.Update(oldSelectedModel);
        }
        _selected.Selected = true;
        _repository.Update(_selected);

        // Update cached value
        _application.Context[ApplicationModelKey] = _selected;
    }

    public ModelEntity[] List()
        => [.. _repository.GetAll().OrderBy(m => m.Name)];

    public ModelEntity? GetByKey(string key)
        => _repository.FindByKey(key);

    public ModelEntity Create(Action<ModelEntity> setUp) {
        var model = new ModelEntity();
        setUp(model);
        return model;
    }

    public void Register(ModelEntity model) {
        if (_repository.FindByKey(model.Key) is not null) {
            throw new InvalidOperationException($"A model with the key '{model.Key}' already exists.");
        }
        if (_selected is null) model.Selected = true;
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
        var model = _repository.FindByKey(key, false) ?? throw new InvalidOperationException($"Settings with key '{key}' not found.");

        _repository.Remove(key);
        _logger.LogInformation("Removed model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public ModelEntity[] ListByProvider(string provider) => _repository.GetFromProvider(provider);

    public void RemoveByProvider(string provider) {
        foreach (var model in _repository.GetFromProvider(provider)) {
            _repository.Remove(model.Key);
            _logger.LogInformation("Removed model associated with provider {Provider}: {ModelKey} => {ModelName}", provider, model.Key, model.Name);
        }
    }

    public void Select(string key) {
        var model = _repository.FindByKey(key)
                 ?? throw new InvalidOperationException($"Settings '{key}' not found.");
        Internal = model;
        _logger.LogInformation("Settings '{ModelKey} => {ModelName}' selected : ", model.Key, model.Name);
    }
}
