namespace Sophia.WebApp.Services;

public class ChatsService(DataContext dbContext)
    : IChatsService {
    public async Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        try {
            var filterClause = ChatData.BuildFilter(filter);
            return await dbContext.Chats
                                  .Include(c => c.Agents).ThenInclude(i => i.Persona)
                                  .Include(c => c.Agents).ThenInclude(i => i.Model)
                                  .AsNoTracking()
                                  .Where(filterClause)
                                  .ToArrayAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<ChatData?> GetById(Guid id) {
        try {
            var entity = await dbContext.Chats
                                        .Include(c => c.Agents).ThenInclude(i => i.Persona).ThenInclude(i => i.KnownTools)
                                        .Include(c => c.Agents).ThenInclude(i => i.Model)
                                        .Include(c => c.Messages)
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .FirstOrDefaultAsync(s => s.Id == id);
            return entity;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Create(ChatData chat) {
        try {
            await dbContext.Chats.Add(chat);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Archive(Guid id) {
        try {
            var entity = await dbContext.Chats.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return;
            entity.IsActive = false;
            await dbContext.Chats.Update(entity);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Unarchive(Guid id) {
        try {
            var entity = await dbContext.Chats.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return;
            entity.IsActive = true;
            await dbContext.Chats.Update(entity);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Rename(Guid id, string newName) {
        try {
            var entity = await dbContext.Chats.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return;
            entity.Title = newName;
            await dbContext.Chats.Update(entity);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task AddMessage(Guid id, MessageData message) {
        try {
            var entity = await dbContext.Chats
                                        .Include(c => c.Messages)
                                        .FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return;
            entity.Messages.Add(message);
            await dbContext.Chats.Update(entity);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Delete(Guid id) {
        try {
            var entity = await dbContext.Chats.FirstOrDefaultAsync(s => s.Id == id);
            if (entity is null) return;
            await dbContext.Chats.Remove(entity);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
