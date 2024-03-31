namespace Sophia.Data.Chats;

public class ChatsRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ChatData, ChatEntity>(dataContext, dbContext) {
    protected override Expression<Func<ChatEntity, bool>> Translate(Expression<Func<ChatData, bool>> predicate) {
        var parameter = Expression.Parameter(typeof(ChatEntity), "entity");
        var body = Expression.Invoke(predicate, Expression.Property(parameter, "Data"));
        return Expression.Lambda<Func<ChatEntity, bool>>(body, parameter);
    }

    protected override Expression<Func<ChatEntity, ChatData>> Project { get; } =
        input => new() {
            Id = input.Id,
            Title = input.Title,
            IsActive = input.IsActive,
            Agents = input.Agents.ToList(Mapper.ToChatAgentData),
            Instructions = Mapper.ToInstructionsData(input.Instructions),
            Messages = input.Messages.ToList(Mapper.ToMessageData),
        };

    protected override Action<ChatData, ChatEntity> UpdateFrom { get; } =
        (input, target) => {
            target.IsActive = input.IsActive;
            target.Title = input.Title;
            target.Agents = input.Agents.ToList(Mapper.ToChatAgentEntity);
            target.Instructions = Mapper.ToInstructionsEntity(input.Instructions);
            target.Messages = input.Messages.ToList(i => Mapper.ToMessageEntity(i));
        };
}
