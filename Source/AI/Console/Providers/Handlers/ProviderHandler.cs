using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace AI.Sample.Providers.Handlers;

public class ProviderHandler(IProviderRepository repository, Lazy<IModelHandler> modelHandler, ILogger<ProviderHandler> logger)
    : IProviderHandler {
    private readonly IProviderRepository _repository = repository;
    private readonly Lazy<IModelHandler> _modelHandler = modelHandler;
    private readonly ILogger<ProviderHandler> _logger = logger;
    public ProviderEntity[] List() => _repository.GetAll();

    public ProviderEntity? GetByKey(uint key) => _repository.FindByKey(key);
    public ProviderEntity? GetByName(string name) => _repository.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public Task<ProviderEntity> Create(Func<ProviderEntity, CancellationToken, Task> setUp, CancellationToken ct = default)
        => _repository.CreateAsync((p, t) => setUp(p, t), ct);

    public void Add(ProviderEntity provider) {
        if (GetByKey(provider.Key) != null)
            throw new ValidationException($"A provider with the key '{provider.Key}' already exists.");

        var context = Map.FromValue(nameof(ProviderHandler), this);
        var result = provider.Validate(context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);

        _repository.Update(provider);
        _logger.LogInformation("Added new provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }

    public void Update(ProviderEntity provider) {
        if (GetByKey(provider.Key) is null)
            throw new ValidationException($"Provider with key '{provider.Key}' not found.");

        var context = Map.FromValue(nameof(ProviderHandler), this);
        var result = provider.Validate(context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);

        _repository.Update(provider);
        _logger.LogInformation("Updated provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }

    public void Remove(uint key) {
        var provider = GetByKey(key) ?? throw new ValidationException($"Provider with key '{key}' not found.");

        _modelHandler.Value.RemoveByProvider(provider.Name);
        _logger.LogInformation("Removed all models associated with provider: {ProviderKey}", key);

        _repository.Remove(key);
        _logger.LogInformation("Removed provider: {ProviderKey} => {ProviderName}", provider.Name, provider.Key);
    }
}
