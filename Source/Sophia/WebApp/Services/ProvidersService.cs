namespace Sophia.WebApp.Services;

public class ProvidersService(ApplicationDbContext dbContext) : IProvidersService {
    public async Task<IReadOnlyList<ProviderData>> GetList(string? filter = null)
        => await dbContext.Providers
                           .AsNoTracking()
                           .Include(p => p.Models)
                           .Select(p => p.ToDto())
                           .ToListAsync();

    public async Task<ProviderData?> GetById(int id) {
        var provider = await dbContext.Providers
                                       .AsNoTracking()
                                       .Include(p => p.Models)
                                       .FirstOrDefaultAsync(p => p.Id == id);
        return provider?.ToDto();
    }

    public async Task Add(ProviderData provider) {
        var entity = provider.ToEntity();
        dbContext.Providers.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(ProviderData provider) {
        var entity = await dbContext.Providers
                                     .Include(p => p.Models)
                                     .FirstOrDefaultAsync(p => p.Id == provider.Id);
        if (entity != null) {
            entity.UpdateFrom(provider);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Providers.FindAsync(id);
        if (entity != null) {
            dbContext.Providers.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
