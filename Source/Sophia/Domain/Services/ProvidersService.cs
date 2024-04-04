namespace Sophia.Services;

public class ProvidersService(DataContext dbContext)
    : IProvidersService {
    public async Task<IReadOnlyList<ProviderData>> GetList(string? filter = null) {
        try {
            var list = await dbContext.Providers.ToArrayAsync();
            return list;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<ProviderData?> GetById(int id) {
        try {
            var provider = await dbContext.Providers.FindByKey(id);
            return provider;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Add(ProviderData input) {
        try {
            await dbContext.Providers.Add(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Update(ProviderData input) {
        try {
            await dbContext.Providers.Update(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Delete(int id) {
        try {
            await dbContext.Providers.Remove(id);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
