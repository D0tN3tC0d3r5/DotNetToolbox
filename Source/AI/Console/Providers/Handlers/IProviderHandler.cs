namespace AI.Sample.Providers.Handlers;

public interface IProviderHandler {
    ProviderEntity[] List();
    ProviderEntity? GetByKey(uint key);
    ProviderEntity Create(Action<ProviderEntity> setUp);
    void Add(ProviderEntity provider);
    void Update(ProviderEntity provider);
    void Remove(uint key);
}
