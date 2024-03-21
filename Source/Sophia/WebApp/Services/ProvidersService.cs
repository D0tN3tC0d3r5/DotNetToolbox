namespace Sophia.WebApp.Services;

public class ProvidersService
    : IProvidersService {
    private readonly ApplicationDbContext _dbContext;

    public ProvidersService(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProviderData>> GetList(string? filter = null)
        => await _dbContext.Providers
                           .AsNoTracking()
                           .Include(p => p.Models)
                           .Select(p => p.ToDto())
                           .ToListAsync();

    public async Task<ProviderData?> GetById(int id) {
        var provider = await _dbContext.Providers
                                       .AsNoTracking()
                                       .Include(p => p.Models)
                                       .FirstOrDefaultAsync(p => p.Id == id);
        return provider?.ToDto();
    }

    public async Task Add(ProviderData provider) {
        var entity = provider.ToEntity();
        _dbContext.Providers.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(ProviderData provider) {
        var entity = await _dbContext.Providers
                                     .Include(p => p.Models)
                                     .FirstOrDefaultAsync(p => p.Id == provider.Id);
        if (entity != null) {
            entity.UpdateFrom(provider);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int id) {
        var entity = await _dbContext.Providers.FindAsync(id);
        if (entity != null) {
            _dbContext.Providers.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
