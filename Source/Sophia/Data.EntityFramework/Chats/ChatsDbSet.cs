namespace Sophia.Data.Chats;

public class ChatsDbSet(ApplicationDbContext dbContext) : ChatRepository {
    public override Task<bool> HaveAny(Expression<Func<ChatData, bool>> predicate, CancellationToken ct = default) {
        var translator = new LambdaExpressionConversionVisitor<ChatData, ChatEntity>();
        var newExpression = translator.Translate<Expression<Func<ChatEntity, bool>>>(predicate);
        return dbContext.Chats.AnyAsync(newExpression, ct);
    }

    public override async Task<IReadOnlyList<ChatData>> ToArrayAsync(CancellationToken ct = default)
        => await dbContext.Chats
                          .AsNoTracking()
                          .Include(i => i.Agents).ThenInclude(i => i.Model)
                          .Include(i => i.Agents).ThenInclude(i => i.Persona)
                          .Include(i => i.Messages)
                          .Select(i => Mapper.ToChatData(i))
                          .ToArrayAsync(ct);

    public override async Task<ChatData?> FindByKey(Guid key, CancellationToken ct = default) {
        var entity = await dbContext.Chats
                                    .AsNoTracking()
                                    .Include(i => i.Agents).ThenInclude(i => i.Model)
                                    .Include(i => i.Agents).ThenInclude(i => i.Persona)
                                    .Include(i => i.Messages)
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        return entity == null
            ? null
            : Mapper.ToChatData(entity);
    }

    public override async ValueTask Add(ChatData input, CancellationToken ct = default) {
        var alreadyExists = await dbContext.Chats.AnyAsync(i => i.Id == input.Id, ct);
        if (alreadyExists)
            return;
        var entity = Mapper.ToChatEntity(input);
        await dbContext.Chats.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Update(ChatData input, CancellationToken ct = default) {
        var entity = await dbContext.Chats
                    .Include(i => i.Agents).ThenInclude(i => i.Model)
                    .Include(i => i.Agents).ThenInclude(i => i.Persona)
                    .Include(i => i.Messages)
                    .FirstOrDefaultAsync(i => i.Id == input.Id, ct);
        if (entity == null)
            return;
        Mapper.UpdateChatEntity(input, entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Remove(Guid key, CancellationToken ct = default) {
        var entity = await dbContext.Chats
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        if (entity == null)
            return;
        dbContext.Chats.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
