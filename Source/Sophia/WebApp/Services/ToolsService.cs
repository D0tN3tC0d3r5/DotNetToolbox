namespace Sophia.WebApp.Services;

public class ToolsService(DataContext dbContext)
    : IToolsService {
    public async Task<IReadOnlyList<ToolData>> GetList(string? filter = null)
        => await dbContext.Tools
                          .AsNoTracking()
                          .ToArrayAsync();

    public async Task<ToolData?> GetById(int id) {
        var entity = await dbContext.Tools
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == id);
        return entity;
    }

    public async Task Add(ToolData input) {
        await dbContext.Tools.Add(input);
        await dbContext.SaveChanges();
    }

    public async Task Update(ToolData input) {
        if (await dbContext.Tools.AllAsync(s => s.Id != input.Id))
            return;
        await dbContext.Tools.Update(input);
        await dbContext.SaveChanges();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Tools.FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null)
            return;
        await dbContext.Tools.Remove(entity);
        await dbContext.SaveChanges();
    }
}
