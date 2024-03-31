namespace Sophia.WebApp.Services;

public class ProvidersService(DataContext dbContext)
    : IProvidersService {
    public async Task<IReadOnlyList<ProviderData>> GetList(string? filter = null)
        => await dbContext.Providers
                          .AsNoTracking()
                          .Include(p => p.Models)
                          .ToListAsync();

    public async Task<ProviderData?> GetById(int id) {
        var provider = await dbContext.Providers
                                      .AsNoTracking()
                                      .Include(p => p.Models)
                                      .FirstOrDefaultAsync(p => p.Id == id);
        return provider;
    }

    public async Task Add(ProviderData provider) {
        await dbContext.Providers.Add(provider);
        await dbContext.SaveChanges();
    }

    public async Task Update(ProviderData provider) {
        if (await dbContext.Providers.AllAsync(s => s.Id != provider.Id)) return;
        await dbContext.Providers.Update(provider);
        await dbContext.SaveChanges();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Providers.FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null) return;
        await dbContext.Providers.Remove(entity);
        await dbContext.SaveChanges();
    }
}
