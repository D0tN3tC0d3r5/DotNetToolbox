using System.Linq.Expressions;

namespace Sophia.WebApp.Services;

public class ChatsService(ApplicationDbContext dbContext)
    : IChatsService {
    public async Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        try {
            var filterClause = BuildFilter(filter);
            return await dbContext.Chats
                                  .Include(c => c.Agents).ThenInclude(i => i.Persona)
                                  .Include(c => c.Agents).ThenInclude(i => i.Provider).ThenInclude(i => i.Models)
                                  .AsNoTracking()
                                  .AsSplitQuery()
                                  .Where(filterClause)
                                  .Select(s => s.ToDto())
                                  .ToArrayAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private static Expression<Func<ChatEntity, bool>> BuildFilter(string? filter)
        => filter switch {
            "ShowArchived" => (_) => true,
            _ => c => c.IsActive == true,
        };

    public async Task<ChatData?> GetById(Guid id) {
        try {
            var entity = await dbContext.Chats
                                            .Include(c => c.Agents).ThenInclude(i => i.Persona).ThenInclude(i => i.Facts)
                                            .Include(c => c.Agents).ThenInclude(i => i.Persona).ThenInclude(i => i.Tools)
                                            .Include(c => c.Agents).ThenInclude(i => i.Provider).ThenInclude(i => i.Models)
                                            .Include(c => c.Messages)
                                            .AsNoTracking()
                                            .AsSplitQuery()
                                            .FirstOrDefaultAsync(s => s.Id == id);
            return entity?.ToDto();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Create(ChatData chat) {
        try {
            var entity = chat.ToEntity();
            dbContext.Chats.Add(entity);
            await dbContext.SaveChangesAsync();
            chat.Id = entity.Id;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Archive(Guid id) {
        var entity = await dbContext.Chats
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null) return;
        entity.IsActive = false;
        await dbContext.SaveChangesAsync();
    }

    public async Task Unarchive(Guid id) {
        var entity = await dbContext.Chats
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null) return;
        entity.IsActive = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task Rename(Guid id, string newName) {
        var entity = await dbContext.Chats
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null) return;
        entity.Title = newName;
        await dbContext.SaveChangesAsync();
    }

    public async Task AddMessage(Guid id, MessageData message) {
        var entity = await dbContext.Chats
                                    .Include(c => c.Messages)
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null) return;
        entity.Messages.Add(message.ToEntity(entity));
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id) {
        var entity = await dbContext.Chats
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return;
        dbContext.Chats.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
