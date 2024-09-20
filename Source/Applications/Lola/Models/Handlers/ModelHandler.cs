using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Models.Handlers;

public class ModelHandler(IApplication application, IModelDataSource dataSource, Lazy<IProviderHandler> providerHandler, ILogger<ModelHandler> logger)
    : IModelHandler {
    private const string _applicationModelKey = "ApplicationModel";

    private ModelEntity? _selected;

    public ModelEntity? Selected {
        get => GetSelected();
        private set => SetSelected(IsNotNull(value));
    }

    private ModelEntity? GetSelected() {
        var cachedValue = application.Context.GetValueAs<ModelEntity>(_applicationModelKey);
        _selected = cachedValue ?? dataSource.GetSelected();
        if (_selected is null) return null;
        if (cachedValue is null) application.Context[_applicationModelKey] = _selected;
        return _selected; // Should only return null if the storage is empty or there is no selected model in the storage.
    }

    private void SetSelected(ModelEntity value) {
        if (value.Key == _selected?.Key) return;
        _selected = value;

        // Ensure record uniqueness in storage
        var oldSelectedModel = dataSource.FirstOrDefault(m => m.Selected);
        if (oldSelectedModel is not null && oldSelectedModel.Key != _selected.Key) {
            oldSelectedModel.Selected = false;
            dataSource.Update(oldSelectedModel);
        }
        _selected.Selected = true;
        dataSource.Update(_selected);

        // Update cached value
        application.Context[_applicationModelKey] = _selected;
    }

    public ModelEntity[] List(uint providerKey = 0)
        => [.. dataSource.GetAll(m => m.ProviderId == 0 || m.ProviderId == providerKey).OrderBy(m => m.Name)];

    public ModelEntity? GetById(uint id)
        => dataSource.FindById(id);

    public ModelEntity? Find(Expression<Func<ModelEntity, bool>> predicate)
        => dataSource.Find(predicate);

    public void Add(ModelEntity model) {
        if (_selected is null) model.Selected = true;
        var context = Map.FromMap([new("EntityAction", "Add"), new(nameof(ModelHandler), this), new(nameof(ProviderHandler), providerHandler.Value)]);
        var result = dataSource.Add(model, context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);
        _selected = model;
        logger.LogInformation("Added new model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Update(ModelEntity model) {
        if (dataSource.FindById(model.Id) == null)
            throw new InvalidOperationException($"Model with id '{model.Id}' not found.");

        var context = Map.FromMap([new(nameof(ModelHandler), this), new(nameof(ProviderHandler), providerHandler.Value)]);
        var result = dataSource.Update(model, context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);
        logger.LogInformation("Updated model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public void Remove(uint id) {
        var model = dataSource.FindById(id, false) ?? throw new InvalidOperationException($"Model with id '{id}' not found.");

        dataSource.Remove(id);
        logger.LogInformation("Removed model: {ModelKey} => {ModelName}", model.Key, model.Name);
    }

    public ModelEntity[] ListByProvider(uint providerKey) => dataSource.GetAll(m => m.ProviderId == providerKey);

    public void Select(uint id) {
        var model = dataSource.FindById(id)
                 ?? throw new InvalidOperationException($"Model '{id}' not found.");
        Selected = model;
        logger.LogInformation("Model '{ModelKey} => {ModelName}' selected : ", model.Key, model.Name);
    }
}
