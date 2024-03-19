
namespace Sophia.WebApp.Services;

public class ToolsService(ApplicationDbContext dbContext)
    : IToolsService {
    public async Task<IReadOnlyList<ToolData>> GetList(string? filter = null)
        => await dbContext.Tools
                          .AsNoTracking()
                          .Select(s => s.ToDto()).ToArrayAsync();

    public async Task<ToolData?> GetById(int id) {
        var entity = await dbContext.Tools.AsNoTracking()
                              .FirstOrDefaultAsync(s => s.Id == id);
        return entity?.ToDto();
    }

    public async Task Add(ToolData selectedTool) {
        var entity = selectedTool.ToEntity();
        dbContext.Tools.Add(entity);
        await dbContext.SaveChangesAsync();
        selectedTool.Id = entity.Id;
    }

    public async Task Update(ToolData input) {
        var entity = await dbContext.Tools
                                    .FirstOrDefaultAsync(s => s.Id == input.Id);
        entity?.UpdateFrom(input);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Tools
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return;
        dbContext.Tools.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
