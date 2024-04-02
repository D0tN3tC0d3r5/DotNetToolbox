namespace Sophia.Services;

public class ToolsService(DataContext dbContext)
    : IToolsService {
    public async Task<IReadOnlyList<ToolData>> GetList(string? filter = null) {
        try {
            var list = await dbContext.Tools.ToArrayAsync();
            return list;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<ToolData?> GetById(int id) {
        try {
            var entity = await dbContext.Tools.FindFirst(s => s.Id == id);
            return entity;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task Add(ToolData input) {
        try {
            await dbContext.Tools.Add(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task Update(ToolData input) {
        try {
            await dbContext.Tools.Update(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task Delete(int id) {
        try {
            await dbContext.Tools.Remove(id);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }
}
