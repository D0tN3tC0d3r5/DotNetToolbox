namespace Sophia.Services;

public class ChatsService(DataContext dbContext)
    : IChatsService {
    public async Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        try {
            var filterClause = ChatData.BuildFilter(filter);
            var list = await dbContext.Chats.Where(filterClause).ToArrayAsync();
            return list;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<ChatData?> GetById(Guid id) {
        try {
            var entity = await dbContext.Chats.FindFirst(c => c.Id == id);
            return entity;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Create(ChatData input) {
        try {
            await dbContext.Chats.Add(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Archive(Guid id) {
        try {
            await dbContext.Chats.Update(id, c => c.IsActive = false);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Unarchive(Guid id) {
        try {
            await dbContext.Chats.Update(id, c => c.IsActive = true);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Rename(Guid id, string newName) {
        try {
            await dbContext.Chats.Update(id, c => c.Title = newName);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task AddMessage(Guid id, MessageData newMessage) {
        try {
            await dbContext.Chats.Update(id, c => c.Messages.Add(newMessage));
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Delete(Guid id) {
        try {
            await dbContext.Chats.Remove(id);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
