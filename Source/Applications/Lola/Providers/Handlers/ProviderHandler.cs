using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Providers.Handlers;

public class ProviderHandler(IProviderDataSource dataSource, Lazy<IModelHandler> modelHandler, ILogger<ProviderHandler> logger)
    : IProviderHandler {
    private readonly IProviderDataSource _dataSource = dataSource;
    private readonly Lazy<IModelHandler> _modelHandler = modelHandler;
    private readonly ILogger<ProviderHandler> _logger = logger;
    public ProviderEntity[] List() => _dataSource.GetAll();

    public ProviderEntity? GetByKey(uint key) => _dataSource.FindByKey(key);
    public ProviderEntity? GetByName(string name) => _dataSource.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void Add(ProviderEntity provider) {
        if (GetByKey(provider.Key) != null)
            throw new ValidationException($"A provider with the key '{provider.Key}' already exists.");

        var context = Map.FromValue(nameof(ProviderHandler), this);
        var result = provider.Validate(context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);

        _dataSource.Update(provider);
        _logger.LogInformation("Added new provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }

    public void Update(ProviderEntity provider) {
        if (GetByKey(provider.Key) is null)
            throw new ValidationException($"Provider with key '{provider.Key}' not found.");

        var context = Map.FromValue(nameof(ProviderHandler), this);
        var result = provider.Validate(context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);

        _dataSource.Update(provider);
        _logger.LogInformation("Updated provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }

    public void Remove(uint key) {
        var provider = GetByKey(key) ?? throw new ValidationException($"Provider with key '{key}' not found.");

        var models = _modelHandler.Value.List(provider.Key);
        foreach (var model in models) {
            _modelHandler.Value.Remove(model.Key);
        }
        _logger.LogInformation("Removed all models from provider: {ProviderKey}", key);

        _dataSource.Remove(key);
        _logger.LogInformation("Removed provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }
}
