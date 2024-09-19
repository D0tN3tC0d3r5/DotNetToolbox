using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Providers.Handlers;

public class ProviderHandler(IProviderDataSource dataSource, Lazy<IModelHandler> modelHandler, ILogger<ProviderHandler> logger)
    : IProviderHandler {
    public ProviderEntity[] List() => dataSource.GetAll();

    public ProviderEntity? GetById(uint id) => dataSource.FindByKey(id);
    public ProviderEntity? GetByName(string name) => dataSource.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void Add(ProviderEntity provider) {
        if (GetById(provider.Id) != null)
            throw new ValidationException($"A provider with the id '{provider.Id}' already exists.");

        var context = Map.FromValue(nameof(ProviderHandler), this);
        var result = dataSource.Add(provider, context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);
        logger.LogInformation("Added new provider: {ProviderId} => {ProviderName}", provider.Name, provider.Id);
    }

    public void Update(ProviderEntity provider) {
        if (GetById(provider.Id) is null)
            throw new ValidationException($"Provider with id '{provider.Id}' not found.");

        var context = Map.FromValue(nameof(ProviderHandler), this);
        var result = dataSource.Update(provider, context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);
        logger.LogInformation("Updated provider: {ProviderId} => {ProviderName}", provider.Name, provider.Id);
    }

    public void Remove(uint id) {
        var provider = GetById(id) ?? throw new ValidationException($"Provider with id '{id}' not found.");

        var models = modelHandler.Value.List(provider.Id);
        foreach (var model in models) {
            modelHandler.Value.Remove(model.Id);
        }
        logger.LogInformation("Removed all models from provider: {ProviderId}", id);

        dataSource.Remove(id);
        logger.LogInformation("Removed provider: {ProviderId} => {ProviderName}", provider.Name, provider.Id);
    }
}
