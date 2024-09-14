using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Providers.Handlers;

public interface IProviderHandler {
    ProviderEntity[] List();
    ProviderEntity? GetByKey(uint key);
    ProviderEntity? GetByName(string name);
    Task<ProviderEntity> Create(Func<ProviderEntity, CancellationToken, Task> setUp, CancellationToken ct = default);
    void Add(ProviderEntity provider);
    void Update(ProviderEntity provider);
    void Remove(uint key);
}
