namespace Lola.Providers.Handlers;

public interface IProviderHandler {
    ProviderEntity[] List();
    ProviderEntity? GetById(uint id);
    ProviderEntity? Find(Expression<Func<ProviderEntity, bool>> predicate);
    void Add(ProviderEntity provider);
    void Update(ProviderEntity provider);
    void Remove(uint id);
}
