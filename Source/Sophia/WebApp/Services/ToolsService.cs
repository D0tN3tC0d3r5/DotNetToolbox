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

    public async Task Add(ToolData tool) {
        await dbContext.Tools.Add(tool);
        await dbContext.SaveChanges();
    }

    public async Task Update(ToolData tool) {
        if(await dbContext.Tools.AllAsync(s => s.Id != tool.Id)) return;
        await dbContext.Tools.Update(tool);
        await dbContext.SaveChanges();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Tools.FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return;
        await dbContext.Tools.Remove(entity);
        await dbContext.SaveChanges();
    }
}
